using CPEI_MFG.Model;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPEI_MFG.View
{
    public partial class UnitView : UserControl
    {
        private readonly MyTimer stopwatch;
        private int testTime;
        private bool isEnable;

        public UnitView(TestModel model)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            lbName.Text = $"ttyUSB{model.Index}";
            grMain.Text = $"SLOT {model.Index}";
            testTime = 0;
            stopwatch = new MyTimer((_) =>
            {
                LockAll(true);
                SetText(lbStatus, $"Testing... {testTime++} s", Color.Yellow);
            });
            Model = model;
            RefreshData();
        }
        public TestModel Model { get; }
        public event Action<string> OnInputEnter;
        public event Action OnVerifyClick;
        public event Action<Control> FocusNextControl;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                SetEnabled(value);
            }
        }
        public void StartTest()
        {
            InvokeUtil.SafeInvoke(lbInput, () => lbInput.Text = Model.Input);
            testTime = 0;
            stopwatch.Start(0, 1000);
            Model.RJ45Num++;
            Model.ProbeNum++;
            RefreshData();
        }

        public void LockAll(bool isLock)
        {
            InvokeUtil.SafeInvoke(txtInput, () => txtInput.ReadOnly = isLock);
            InvokeUtil.SafeInvoke(btVerify, () => btVerify.Enabled = !isLock);
        }
        private void SetEnabled(bool enable)
        {
            InvokeUtil.SafeInvoke(txtInput, () => txtInput.Enabled = enable);
            InvokeUtil.SafeInvoke(btVerify, () => btVerify.Enabled = enable);
        }

        public void StoptTest()
        {
            stopwatch.Stop();
            if (Model.TestResult)
            {
                SetText(lbStatus, $"PASS, Time: {testTime}", Color.Green);
                Model.PassNum++;
            }
            else
            {
                SetText(lbStatus, $"FAIL: '{Model.Errorcode}/{Model.Message}', Time: {testTime}", Color.Red);
                Model.FailNum++;
            }
            RefreshData();
        }
        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter || string.IsNullOrEmpty(txtInput.Text))
            {
                return;
            }
            Task.Run(() =>
            {
                Model.Input = txtInput.Text.Trim();
                SetText(lbInput, string.Empty, Color.Black);
                OnInputEnter?.Invoke(Model.Input);
                InvokeUtil.SafeInvoke(txtInput, () =>
                {
                    txtInput.Text = string.Empty;
                    FocusNextControl?.Invoke(txtInput);
                });
            });
        }

        public void RefreshData()
        {
            SetText(lbFail, Model.FailNum.ToString(), Color.Red);
            SetText(lbPass, Model.PassNum.ToString(), Color.Green);
            SetText(lbRJ45, Model.RJ45Num.ToString(), Color.Black);
            SetText(lbProbe, Model.ProbeNum.ToString(), Color.Black);
        }

        public void ShowMessage(string mess)
        {
            SetText(lbStatus, mess, Color.Black);
        }

        public void ShowErrorMess(string mess)
        {
            SetText(lbStatus, mess, Color.Red);
        }


        private void btVerify_Click(object sender, EventArgs e)
        {
            Task.Run(() => OnVerifyClick?.Invoke());
        }

        private void SetText(Control control, string mess, Color color)
        {
            InvokeUtil.SafeInvoke(control, () =>
            {
                control.Text = mess;
                control.ForeColor = color;
            });
        }

        private void lbRJ45_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Model.RJ45Num = 0;
        }

        private void lbProbe_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Model.ProbeNum = 0;
        }
    }
}
