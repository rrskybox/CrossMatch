/*
* Main window for application
* 
* Author:           Rick McAlister
* Date:             12/5/2022
* Current Version:  1.0
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro, .Net 4.8, TSX 5.0 Build 13479
* 
* Change Log:
* 
* 12/5/2022 Rev 1.0  Release
* 
*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using TheSky64Lib;


namespace CrossMatch
{
    public enum HeaderType
    {
        Generic = 0,
        IAU = 1,
        SDBX = 2
    }

    public partial class FormCatalogPlot : Form
    {
        public bool IsWaitingOnStep = false;
        public string[] lines;
        public double magDiff;
        public string listPath;

        private bool abortFlag = false;

        public XDocument DBxml;

        const double StandardMagnitudeRange = 2;
        const double DisableMagnitudeRange = 20;
        const double MinimumReferenceMagnitude = 2;

        public FormCatalogPlot()
        {
            InitializeComponent();
            ButtonGreen(MapButton);
            ButtonGreen(ListButton);
            ButtonDisable(NextButton);
            ButtonDisable(AbortButton);
            ButtonGreen(CloseButton);
            StarChart.Series[0]["BubbleScaleMin"] = "-20";

            try
            { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                this.Text = " in Debug";
            }
            this.Text = "Cross Match V" + this.Text;
            try { sky6StarChart tsxsc = new sky6StarChart(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            HeaderChoiceBox.SelectedIndex = (int)HeaderType.SDBX;

        }

        private List<StarFinder.ReferenceData> FindTargetReferences()
        {
            //Clear current references and charts
            StarChart.Series[0].Points.Clear();
            StarListTreeView.Nodes.Clear();
            //Find current clickfind location
            (double ra, double dec) = StarFinder.GetCurserPosition();
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

        private void ListReferencesCSV(HeaderType hdrType)
        {
            List<TargetData> tgtDataList = new List<TargetData>();

            bool isIAU = false;
            bool raIsDegrees = false;
            if (hdrType == HeaderType.Generic)
            {
                isIAU = false;
                raIsDegrees = false;
            }
            if (hdrType == HeaderType.IAU)
            {
                isIAU = true;
                raIsDegrees = true;
            }

            DialogResult dr = StarListFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get csv filename and translate contents into an XML database
                string listPath = StarListFileDialog.FileName;
                ListRunner tgtInput = new ListRunner(listPath, raIsDegrees);
                for (int i = 0; i < tgtInput.CsvTargetList.Count; i++)
                {
                    //Update star location if required
                    if (UpdateCheckbox.Checked)
                    {
                        (double newRA, double newDec) = StarFinder.LookupStarRADec(tgtInput.CsvTargetList[i].TargetName);
                        if (newRA != 0 && newDec != 0)
                        {
                            tgtInput.CsvTargetList[i].TargetRA = newRA;
                            tgtInput.CsvTargetList[i].TargetDec = newDec;
                        }
                    }
                    magDiff = StandardMagnitudeRange;
                    tgtInput.CsvTargetList[i] = FindReference(tgtInput.CsvTargetList[i], magDiff);
                    while (!SkipBox.Checked && !tgtInput.CsvTargetList[i].HasReference && (tgtInput.CsvTargetList[i].TargetMag >= 3) && !SkipMagCheckBox.Checked)
                    {
                        DialogResult mdHand = MessageBox.Show("Reference failure.  Do you want to hand select?", "Manual Option for " + tgtInput.CsvTargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        if (mdHand == DialogResult.Yes)
                        {
                            DialogResult mdMag = MessageBox.Show("Do you want to ignore magnitudes?", "Manual Option for " + tgtInput.CsvTargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (mdMag == DialogResult.Yes)
                                magDiff = DisableMagnitudeRange;
                            DialogResult mdClick = MessageBox.Show("Click on new target then hit OK", "Manual Option for " + tgtInput.CsvTargetList[i].TargetName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            //Find current clickfind location
                            if (mdClick == DialogResult.OK)
                            {
                                (double ra, double dec) = StarFinder.GetCurserPosition();
                                tgtInput.CsvTargetList[i].TargetRA = ra;
                                tgtInput.CsvTargetList[i].TargetDec = dec;
                                tgtInput.CsvTargetList[i] = FindReference(tgtInput.CsvTargetList[i], magDiff);
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
                        if (abortFlag)
                            return;
                    }
                    ButtonDisable(NextButton);
                    ButtonRed(ListButton);
                    System.Windows.Forms.Application.DoEvents();
                    if (abortFlag)
                        return;
                }
                //write out new file -- Rev 15 restricted to csv file input and and sdb text output
                if (Path.GetExtension(listPath).Contains("csv"))
                {
                    SDBDesigner sdb = new SDBDesigner();
                    sdb.SDBToClipboard(tgtInput.CsvTargetList, isIAU);
                    sdb.SDBToCSVFile(tgtInput.CsvTargetList, listPath, isIAU);
                }
                int refCount = (tgtInput.CsvTargetList.Where(x => x.HasReference)).Count();
                MessageBox.Show("Collected " + refCount + "/" + tgtInput.CsvTargetList.Count.ToString() + " " + RefTextBox.Text + " reference stars", "List Completion", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

        }

        private void ListReferencesSDB(HeaderType hdrType)
        {
            List<TargetData> tgtDataList = new List<TargetData>();

            DialogResult dr = StarListFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get csv filename and translate contents into an XML database
                string listPath = StarListFileDialog.FileName;
                SdbRunner tgtInput = new SdbRunner(listPath);
                for (int i = 0; i < tgtInput.SdbTargetList.Count; i++)
                {
                    magDiff = StandardMagnitudeRange;
                    tgtInput.SdbTargetList[i] = FindReference(tgtInput.SdbTargetList[i], magDiff);
                    while (! SkipBox.Checked && !tgtInput.SdbTargetList[i].HasReference && (tgtInput.SdbTargetList[i].TargetMag >= MinimumReferenceMagnitude) && !SkipMagCheckBox.Checked)
                    {
                        DialogResult mdHand = MessageBox.Show("Reference failure.  Do you want to hand select?", "Manual Option for " + tgtInput.SdbTargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        if (mdHand == DialogResult.Yes)
                        {
                            DialogResult mdMag = MessageBox.Show("Do you want to ignore magnitudes?", "Manual Option for " + tgtInput.SdbTargetList[i].TargetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            if (mdMag == DialogResult.Yes)
                                magDiff = DisableMagnitudeRange;
                            DialogResult mdClick = MessageBox.Show("Click on new target then hit OK", "Manual Option for " + tgtInput.SdbTargetList[i].TargetName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            //Find current clickfind location
                            if (mdClick == DialogResult.OK)
                            {
                                (double ra, double dec) = StarFinder.GetCurserPosition();
                                tgtInput.SdbTargetList[i].TargetRA = ra;
                                tgtInput.SdbTargetList[i].TargetDec = dec;
                                tgtInput.SdbTargetList[i] = FindReference(tgtInput.SdbTargetList[i], magDiff);
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
                        if (abortFlag)
                            return;
                    }
                    ButtonDisable(NextButton);
                    ButtonRed(ListButton);
                    System.Windows.Forms.Application.DoEvents();
                    if (abortFlag)
                        return;
                }
                //write out new file
                tgtInput.AppendTargetData(listPath);
                int refCount = (tgtInput.SdbTargetList.Where(x => x.HasReference)).Count();
                MessageBox.Show("Collected " + refCount + "/" + tgtInput.SdbTargetList.Count.ToString() + " " + RefTextBox.Text + " reference stars", "List Completion", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private TargetData FindReference(TargetData tgtData, double magDiff)
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
            ButtonEnable(AbortButton);
            ButtonGreen(AbortButton);

            abortFlag = false;
            HeaderType hType;

            Enum.TryParse(HeaderChoiceBox.Text, out hType);

            switch (hType)
            {
                case HeaderType.Generic:
                    ListReferencesCSV(hType);
                    break;
                case HeaderType.SDBX:
                    ListReferencesSDB(hType);
                    break;
                case HeaderType.IAU:
                    ListReferencesCSV(hType);
                    break;
                default:
                    return;
            }

            ButtonGreen(ListButton);
            ButtonEnable(MapButton);
            ButtonDisable(NextButton);
            ButtonDisable(AbortButton);
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

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OnTopCheckBox.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }
        private void AbortButton_Click(object sender, EventArgs e)
        {
            abortFlag = true;
        }
        #endregion

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }


    }

}
