using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using USPSAddressVerifier.Lib;
using System.Diagnostics;
using Microsoft.Win32;

namespace USPSAddressVerifierDesktop
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;

            //txtFile_DoubleClick(null, null);

            ReloadFormConfigurationValues();
        }


        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Are you sure you want to start processing?", "Confirm", MessageBoxButtons.OKCancel))
            {
                DateTime dtStart = DateTime.Now;
                Cursor.Current = Cursors.WaitCursor;

                //PreProcess();

                ProcessThings(null, null);

                Cursor.Current = Cursors.Default;
                DateTime dtEnd = DateTime.Now;
                TimeSpan ts = (dtEnd - dtStart);

                string timeoutput = "";

                double totalminutes = ts.TotalMinutes;

                if (ts.TotalMinutes >= 100)
                {
                    timeoutput = ts.TotalHours.ToString() + " hours and " + Math.Round((ts.TotalMinutes / 60.0), 2).ToString() + " minute(s)";
                }
                else if (ts.TotalMinutes >= 1)
                {
                    timeoutput = Math.Round(ts.TotalMinutes, 2).ToString() + " minute(s)";
                }
                else if (ts.TotalMinutes < 1)
                {
                    timeoutput = Math.Round(ts.TotalSeconds).ToString() + " seconds(s)";
                }
                else
                {
                    timeoutput = Math.Round((ts.TotalMinutes / 60.0), 2).ToString() + " minute(s)";
                }

                MessageBox.Show("Execution completed in: " + timeoutput, "Execution Completed", MessageBoxButtons.OK);
            }
        }

        private void btnProcessWithBackgroundWorker_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(ProcessThings);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync(comboBox1.SelectedIndex);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);

            Cursor.Current = Cursors.Default;
        }

        public void SetTextBoxValue(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetTextBoxValue), new object[] { value });
                return;
            }

            txtErrors.Text = value;
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pnlErrors.Visible = true;
        }

        public void ProcessThings(object sender, DoWorkEventArgs a)
        {
            btnViewReturnFile.Visible = false;

            //System.Threading.Thread.Sleep(5000);
            pnlErrors.Visible = false;

            int selectedIndex = -1;

            if (a==null)
            { 
                selectedIndex = comboBox1.SelectedIndex;
            }
            else
            {
                selectedIndex = Convert.ToInt32(a.Argument.ToString());
            }           

            if (errorProvider1.GetError(txtFile).Length == 0 && errorProvider1.GetError(comboBox1).Length == 0)
            {
                //pnlErrors.Visible = false;

                int? pmdclientid = null;

                int newint;

                if(Int32.TryParse(txtPMDClientID.Text,out newint))
                {
                    pmdclientid = Convert.ToInt32(txtPMDClientID.Text);
                }

                Processor p = new Processor();
                string filename = txtFile.Text;
                
                if (p.ValidateArguments(filename, selectedIndex))
                {
                    if (selectedIndex == 0)
                    {
                        p.FullFileAddressMode(filename, 1);
                        btnViewReturnFile.Visible = true;
                        lblReturnFile.Text = p.ReturnFileName;
                    }
                    else if (selectedIndex == 1)
                    {
                        p.FullFileAddressMode(filename, 2);
                        btnViewReturnFile.Visible = true;
                        lblReturnFile.Text = p.ReturnFileName;
                    }
                    else if (selectedIndex==2)
                    {
                        p.DatabaseLooper(pmdclientid, null);
                        btnViewReturnFile.Visible = true;
                        lblReturnFile.Text = p.ReturnFileName;
                    }
                    else
                    {
                        txtErrors.Text = "An error occurred while picking your File Mode. Please try again.";
                        pnlErrors.Visible = true;
                    }
                }
                else
                {
                }

                pnlErrors.Visible = true;
                txtErrors.Text = p.AddressCheckOutput;

                SetTextBoxValue(p.AddressCheckOutput);
            }
            else
            {
                //pnlErrors.Visible = false;
            }
        }


        private void txtFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnViewSample_Click(object sender, EventArgs e)
        {
            try
            {
                //pnlErrors.Visible = false;

                Process p = new Process();
                p.StartInfo.FileName = @"C:\Users\brian.williams.PHARMMD\Documents\Visual Studio 2008\Projects\USPSAddressVerifier\USPSAddressVerifierDesktop\sample address file.txt";
                p.Start();
            }
            catch(Exception ex)
            {
                pnlErrors.Visible = true;
                txtErrors.Text = "An error occurred while trying to open this file: " + ex.Message + " - " + ex.StackTrace;
            }

}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                errorProvider1.SetError(txtFile, "");
            }
            else
            {
                if(txtFile.Text=="")
                {
                    errorProvider1.SetError(txtFile, "You must select a file");
                }

                if (comboBox1.SelectedItem.ToString() != "")
                {
                    errorProvider1.SetError(comboBox1, "");
                }
                else
                {
                    errorProvider1.SetError(comboBox1, "File Mode is required");
                }
            }
        }

        private void txtFile_DoubleClick(object sender, EventArgs e)
        {
            ShowFileSelectDialog();
        }

        internal void ShowFileSelectDialog()
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                txtFile.Text = openFileDialog1.FileName;
            }

            if (txtFile.Text != "")
            {
                errorProvider1.SetError(txtFile, "");
            }
            else
            {
                errorProvider1.SetError(txtFile, "You must select a file");
            }
        }


        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        internal void ReloadFormConfigurationValues()
        {
            try
            {
                string subkeyname = "Software";
                string appname = "USPSAddressVerifier";

                RegistryKey key = Registry.LocalMachine.OpenSubKey(subkeyname, true);
                RegistryKey tfmKey = key.OpenSubKey(appname, RegistryKeyPermissionCheck.ReadWriteSubTree);

                if (tfmKey != null)
                {
                    if (tfmKey.ValueCount > 0)
                    {
                        if (tfmKey.GetValue("FileName") != null)
                        {
                            txtFile.Text = tfmKey.GetValue("FileName").ToString();
                        }
                        
                    }
                }
            }
            catch { }
        }

        private void btnCopyResultsToClipboard_Click(object sender, EventArgs e)
        {
            if (txtErrors.Text.Length > 0)
            {
                Clipboard.SetText(txtErrors.Text);
                MessageBox.Show("Results copied successfully.");
            }
            else
            {
                 MessageBox.Show("Nothing to copy!");
            }
        }

        private void btnViewReturnFile_Click(object sender, EventArgs e)
        {
            try
            {
                //pnlErrors.Visible = false;

                Process p = new Process();
                p.StartInfo.FileName = @"" + lblReturnFile.Text;
                p.Start();
            }
            catch(Exception ex)
            {
                pnlErrors.Visible = true;
                txtErrors.Text = "An error occurred while trying to open this file: " + ex.Message + " - " + ex.StackTrace;
            }
        }

        internal List<long> CurrentPatientSentList()
        {
            PharmMDFinal.Lib.Processor p = new PharmMDFinal.Lib.Processor();
            return p.ValidatedPatientList();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            ShowFileSelectDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButtons.OKCancel))
            {

                Application.Exit();
            }
        }
    }
}
