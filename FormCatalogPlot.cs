using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using TheSky64Lib;


namespace GaiaReferral
{
    public partial class FormCatalogPlot : Form
    {
        public bool IsWaitingOnStep = false;
        public string[] lines;
        public int refCount;
        public double magDiff;
        public string listPath;

        public XDocument DBxml;

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

        private List<StarFinder.ReferenceData> FindTargetReferences()
        {
            //Find current clickfind location
            (double ra, double dec) = StarFinder.LookupClickFind();
            //Read in each star object and save name, ra and dec, magnitude and id's
            List<StarFinder.ReferenceData> sdList = StarFinder.FindNearbyStars(ra, dec);
            if (sdList.Count > 0)
            {
                List<StarFinder.ReferenceData> sdNorm = NormalizeLocation(sdList);
                PlotStars(sdNorm);
                CreateStarReferenceTree(sdNorm);
            }
            return sdList;
        }

        #region star graph plotting
        private void PlotStars(List<StarFinder.ReferenceData> sdNorm)
        {
            //Clear chart points and add new ones
            StarChart.Series[0].Points.Clear();
            for (int i = 0; i < sdNorm.Count; i++)
            {
                int idx = StarChart.Series[0].Points.AddXY(sdNorm[i].NormalDec, sdNorm[i].NormalRA, -sdNorm[i].Magnitude);
                StarChart.Series[0].Points[idx].Label = "(" + i.ToString() + ") " + sdNorm[i].ReferenceName;
                Show();
            }
        }

        private void CreateStarReferenceTree(List<StarFinder.ReferenceData> sd)
        {
            //Clear, then load the starListing with the recovered catalog references
            string id;
            StarListTreeView.Nodes.Clear();

            for (int i = 0; i < sd.Count; i++)
            {
                int main, leaf, tempLeaf;
                main = AddMainNode("(" + i.ToString() + ") " + sd[i].ReferenceName);
                if (sd[i].ReferenceName.Contains("Jpass"))
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

        private int AddLeafNode(int node, string leafString)
        {
            TreeNode lNode = StarListTreeView.Nodes[node].Nodes.Add(leafString);
            return StarListTreeView.Nodes.IndexOf(lNode);
        }

        private List<StarFinder.ReferenceData> NormalizeLocation(List<StarFinder.ReferenceData> sdRaw)
        {
            //Translate RA/Dec to relative positions
            List<StarFinder.ReferenceData> sdNorm = new List<StarFinder.ReferenceData>();
            //Calculate averages for ra/dec points, then normalize to average
            double raAvg = sdRaw.Average(x => x.RA);
            double decAvg = sdRaw.Average(x => x.Dec);
            for (int i = 0; i < sdRaw.Count; i++)
            {
                StarFinder.ReferenceData sd = sdRaw[i];
                sd.NormalRA = (sd.RA - raAvg) * 3600;
                sd.NormalDec = (sd.Dec - decAvg) * 3600;

                sdNorm.Add(sd);
            }
            return sdNorm;
        }

        private int AddMainNode(string section)
        {
            TreeNode mNode = StarListTreeView.Nodes.Add(section);
            return StarListTreeView.Nodes.IndexOf(mNode);
        }
        #endregion

        private void ListReferences()
        {
            List<ListRunner.TargetData> tgtDataList = new List<ListRunner.TargetData>();

            DialogResult dr = StarListFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get csv filename and translate contents into an XML database
                string listPath = StarListFileDialog.FileName;
                ListRunner tgtInput = new ListRunner(listPath);
                refCount = 0;
                for (int i = 0; i < tgtInput.TargetList.Count; i++)
                {
                    magDiff = StandardMagnitudeRange;
                    tgtInput.TargetList[i] = FindReference(tgtInput.TargetList[i], magDiff);
                    while (!tgtInput.TargetList[i].HasReference && (tgtInput.TargetList[i].TargetMag >= 3) && !SkipCheckBox.Checked)
                    {
                        DialogResult mdHand = MessageBox.Show("Reference failure.  Do you want to hand select?", "Manual Option for " + tgtInput.TargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        if (mdHand == DialogResult.Yes)
                        {
                            DialogResult mdMag = MessageBox.Show("Do you want to ignore magnitudes?", "Manual Option for " + tgtInput.TargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (mdMag == DialogResult.Yes)
                                magDiff = 8;
                            DialogResult mdClick = MessageBox.Show("Click on new target then hit OK", "Manual Option for " + tgtInput.TargetList[i].TargetName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            //Find current clickfind location
                            if (mdClick == DialogResult.OK)
                            {
                                (double ra, double dec) = StarFinder.LookupClickFind();
                                tgtInput.TargetList[i].TargetRA = ra;
                                tgtInput.TargetList[i].TargetDec = dec;
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
                //write out new file -- Rev 15 restricted to csv file input and and sdb text output
                if (Path.GetExtension(listPath).Contains("csv"))
                {
                    //string newFileName = listPath.Substring(0, listPath.Length - 4) + ".new.csv";
                    //File.WriteAllLines(newFileName, outLines);
                    //ReadReferral rRef = new ReadReferral(lines[0]);
                    SDBDesigner sdb = new SDBDesigner();
                    sdb.SDBToClipboard(tgtInput.TargetList);
                    sdb.SDBToCSVFile(tgtInput.TargetList, listPath);
                }
                MessageBox.Show("Collected " + refCount + " " + RefTextBox.Text + "reference stars", "List Completion", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

        }

        private ListRunner.TargetData FindReference(ListRunner.TargetData tgtData, double magDiff)
        {
            StarFinder.SetStarChart(tgtData.TargetRA, tgtData.TargetDec);
            List<StarFinder.ReferenceData> sdList = StarFinder.FindNearbyStars(tgtData.TargetRA, tgtData.TargetDec);
            if (sdList.Count > 0)
            {
                List<StarFinder.ReferenceData> sdNorm = NormalizeLocation(sdList);
                sdNorm = StarFinder.ComputeAllSeparations(sdNorm, tgtData.TargetRA, tgtData.TargetDec);
                PlotStars(sdNorm);
                CreateStarReferenceTree(sdNorm);
                Show();

                StarFinder.ReferenceData? sdg = StarFinder.FindNearestByCatalog(sdNorm, RefTextBox.Text, tgtData.TargetMag, magDiff);
                if (sdg != null)
                {
                    tgtData.CrossRefName = ((StarFinder.ReferenceData)sdg).ReferenceName + " " + ((StarFinder.ReferenceData)sdg).CatalogId;
                    tgtData.ReferenceStar = (StarFinder.ReferenceData)sdg;
                    tgtData.HasReference = true;
                    refCount++;
                    return tgtData;
                }
                else
                {
                    tgtData.CrossRefName = "None";
                    tgtData.HasReference = false;
                }
            }
            return tgtData;
        }

         #region click events
        private void FindButton_Click(object sender, EventArgs e)
        {
            ButtonRed(MapButton);
            ButtonDisable(ListButton);
            ButtonDisable(NextButton);

            FindTargetReferences();

            ButtonGreen(MapButton);
            ButtonEnable(ListButton);
            ButtonDisable(NextButton);
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            ButtonDisable(MapButton);
            ButtonRed(ListButton);
            ButtonDisable(NextButton);

            ListReferences();

            ButtonGreen(ListButton);
            ButtonEnable(MapButton);
            ButtonDisable(NextButton);
            return;
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

        #endregion

    }

}
