using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TheSky64Lib;


namespace GaiaReferral
{
    public partial class FormCatalogPlot : Form
    {
        public bool IsWaitingOnStep = false;
        public string[] lines;
        public string[] outLines;
        public int refCount;
        public double magDiff;
        public string listPath;

        const double StandardMagnitudeRange = 2;

        public FormCatalogPlot()
        {
            InitializeComponent();
            sky6StarChart tsxsc = new sky6StarChart();
            StarChart.Series[0]["BubbleScaleMin"] = "-20";
            ButtonGreen(MapButton);
            ButtonGreen(ListButton);
            ButtonDisable(NextButton);
            try
            { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                this.Text = " in Debug";
            }
            this.Text = "Star Tours V" + this.Text;

        }

        private List<StarFinder.starData> FindTarget()
        {
            //Find current clickfind location
            (double ra, double dec) = StarFinder.LookupClickFind();
            //Read in each star object and save name, ra and dec, magnitude and id's
            List<StarFinder.starData> sdList = StarFinder.FindNearbyStars(ra, dec);
            if (sdList.Count > 0)
            {
                List<StarFinder.starData> sdNorm = NormalizeLocation(sdList);
                PlotStars(sdNorm);
                LoadStarListing(sdNorm);
            }
            return sdList;
        }

        private void PlotStars(List<StarFinder.starData> sdNorm)
        {
            //Clear chart points and add new ones
            StarChart.Series[0].Points.Clear();
            for (int i = 0; i < sdNorm.Count; i++)
            {
                int idx = StarChart.Series[0].Points.AddXY(sdNorm[i].NormalDec, sdNorm[i].NormalRA, -sdNorm[i].Magnitude);
                StarChart.Series[0].Points[idx].Label = "(" + i.ToString() + ") " + sdNorm[i].TargetName;
                Show();
            }
        }

        private List<StarFinder.starData> NormalizeLocation(List<StarFinder.starData> sdRaw)
        {
            //Translate RA/Dec to relative positions
            List<StarFinder.starData> sdNorm = new List<StarFinder.starData>();
            //Calculate averages for ra/dec points, then normalize to average
            double raAvg = sdRaw.Average(x => x.RA);
            double decAvg = sdRaw.Average(x => x.Dec);
            for (int i = 0; i < sdRaw.Count; i++)
            {
                StarFinder.starData sd = sdRaw[i];
                sd.NormalRA = (sd.RA - raAvg) * 3600;
                sd.NormalDec = (sd.Dec - decAvg) * 3600;

                sdNorm.Add(sd);
            }
            return sdNorm;
        }

        private void LoadStarListing(List<StarFinder.starData> sd)
        {
            //Clear, then load the starListing with the recovered catalog references
            string id;
            StarListTreeView.Nodes.Clear();

            for (int i = 0; i < sd.Count; i++)
            {
                int main, leaf, locLeaf, tempLeaf;
                main = AddMainNode("(" + i.ToString() + ") " + sd[i].TargetName);
                if (sd[i].TargetName.Contains("Jpass"))
                    id = sd[i].FieldId;
                else
                    id = sd[i].CatalogId;
                leaf = AddLeafNode(main, "Cat Id: " + id);
                tempLeaf = AddLeafNode(main, "RA: " + sd[i].RA.ToString());
                tempLeaf = AddLeafNode(main, "Dec: " + sd[i].Dec.ToString());
                tempLeaf = AddLeafNode(main, "Magnitude: " + sd[i].Magnitude.ToString());
                tempLeaf = AddLeafNode(main, "RA Separation: " + sd[i].NormalRA.ToString("0.0"));
                tempLeaf = AddLeafNode(main, "Dec Separation: " + sd[i].NormalDec.ToString("0.0"));
            }
            Show();
        }

        private int AddMainNode(string section)
        {
            TreeNode mNode = StarListTreeView.Nodes.Add(section);
            return StarListTreeView.Nodes.IndexOf(mNode);
        }

        private int AddLeafNode(int node, string leafString)
        {
            TreeNode lNode = StarListTreeView.Nodes[node].Nodes.Add(leafString);
            return StarListTreeView.Nodes.IndexOf(lNode);
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            ButtonRed(MapButton);
            ButtonDisable(ListButton);
            ButtonDisable(NextButton);

            FindTarget();

            ButtonGreen(MapButton);
            ButtonEnable(ListButton);
            ButtonDisable(NextButton);
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            ButtonDisable(MapButton);
            ButtonRed(ListButton);
            ButtonDisable(NextButton);

            DialogResult dr = StarListFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //get filename and open textfile for reading
                listPath = StarListFileDialog.FileName;
                lines = File.ReadAllLines(listPath);
                outLines = lines;
                if (Path.GetExtension(listPath).Contains("csv"))
                    outLines[0] += ",GAIA Reference, GAIA RA, GAIA Dec";
                else
                    outLines[0] += "     GAIA Reference                  GAIA RA               GAIA Dec              Magnitude";
                refCount = 0;
                for (int i = 1; i < lines.Length; i++)
                {
                    ListRunner.Referral lr = new ListRunner.Referral();
                    if (Path.GetExtension(listPath).Contains("csv"))
                        lr = ListRunner.BuildReferenceCSV(lines[i]);
                    else
                        lr = ListRunner.BuildReference(lines[i]);
                    magDiff = StandardMagnitudeRange;
                    while (!FindReference(i, lr, magDiff) && (lr.Magnitude >= 3))
                    {
                        DialogResult mdHand = MessageBox.Show("Reference failure.  Do you want to hand select?", "Manual Option for " + lr.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        if (mdHand == DialogResult.Yes)
                        {
                            DialogResult mdMag = MessageBox.Show("Do you want to ignore magnitudes?", "Manual Option for " + lr.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (mdMag == DialogResult.Yes)
                                magDiff = 8;
                            DialogResult mdClick = MessageBox.Show("Click on new target then hit OK", "Manual Option for " + lr.Name, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            //Find current clickfind location
                            if (mdClick == DialogResult.OK)
                            {
                                (double ra, double dec) = StarFinder.LookupClickFind();
                                lr.RA = ra;
                                lr.Dec = dec;
                            }
                        }
                        else
                            break;
                    }
                    if (StepCheckBox.Checked)
                        IsWaitingOnStep = true;
                    while (IsWaitingOnStep)
                    {
                        ButtonEnable(NextButton);
                        ButtonGreen(NextButton);
                        System.Threading.Thread.Sleep(1000);
                        Show();
                        System.Windows.Forms.Application.DoEvents();
                    }
                    ButtonDisable(NextButton);
                    ButtonRed(ListButton);
                }
                //write out new file
                if (Path.GetExtension(listPath).Contains("csv"))
                {
                    string newFileName = listPath.Substring(0, listPath.Length - 4) + ".new.csv";
                    File.WriteAllLines(newFileName, outLines);
                }
                else
                    File.WriteAllLines(listPath + ".new", outLines);
                MessageBox.Show("Collected " + refCount + " GAIA reference stars", "List Completion", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            ButtonGreen(ListButton);
            ButtonEnable(MapButton);
            ButtonDisable(NextButton);
            return;
        }

        private bool FindReference(int lineIdx, ListRunner.Referral lr, double magDiff)
        {
            StarFinder.SetStarChart(lr.RA, lr.Dec);
            List<StarFinder.starData> sdList = StarFinder.FindNearbyStars(lr.RA, lr.Dec);
            if (sdList.Count > 0)
            {
                List<StarFinder.starData> sdNorm = NormalizeLocation(sdList);
                sdNorm = StarFinder.ComputeAllSeparations(sdNorm, lr.RA, lr.Dec);
                PlotStars(sdNorm);
                LoadStarListing(sdNorm);
                Show();

                StarFinder.starData? sdg = StarFinder.FindNearestByCatalog(sdNorm, "Gaia", lr.Magnitude, magDiff);
                if (sdg != null)
                {
                    lr.CrossGaiaName = ((StarFinder.starData)sdg).TargetName + " " + ((StarFinder.starData)sdg).CatalogId.PadRight(19);
                    if (Path.GetExtension(listPath).Contains("csv"))
                        outLines[lineIdx] += "," + lr.CrossGaiaName + "," + ((StarFinder.starData)sdg).RA + "," + ((StarFinder.starData)sdg).Dec + "," + ((StarFinder.starData)sdg).Magnitude;
                    else
                        outLines[lineIdx] += "    " + lr.CrossGaiaName + "    " + ((StarFinder.starData)sdg).RA.ToString().PadRight(18) + "    " + ((StarFinder.starData)sdg).Dec.ToString().PadRight(18) + "    " + ((StarFinder.starData)sdg).Magnitude.ToString().PadRight(8);
                    refCount++;
                    return true;
                }
            }
            return false;
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            IsWaitingOnStep = false;
        }

        private void ButtonRed(Button bt)
        {
            bt.BackColor = Color.LightSalmon;
        }

        private void ButtonGreen(Button bt)
        {
            bt.BackColor = Color.LightGreen;
        }

        private void ButtonDisable(Button bt)
        {
            bt.Enabled = false;
            bt.BackColor = Color.Gray;
        }

        private void ButtonEnable(Button bt)
        {
            bt.Enabled = true;
            bt.BackColor = Color.LightGreen;
        }

    }

}
