using System;
using System.Linq;
using System.Windows.Forms;
using EmployeesAffairsProject.Context;

namespace EmployeesAffairsProject.Forms
{
    public partial class UniversityForm : Form
    {
        private User TUser;
        private int universityID = 0;

        public UniversityForm(User oUser)
        {
            InitializeComponent();
            this.Text = $"{this.Text}--{oUser.userName}";
            TUser = oUser;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            universityID = 0;
            TxtUniversityName.Clear();
            TxtUniversityName.Focus();
        }

        private void UniversityForm_Load(object sender, EventArgs e)
        {
            LoadUniversities();
        }

        private void LoadUniversities()
        {
            using (UDBEntities dbContext = new UDBEntities())
            {
                var universities = dbContext.University.ToList();
                DgvUniversities.DataSource = universities;

                ConfigureDataGridViewColumns();
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            DgvUniversities.Columns[nameof(University.universityID)].Visible = false;
            DgvUniversities.Columns[nameof(University.user_id)].Visible = false;
            DgvUniversities.Columns[nameof(University.UniversityBranch)].Visible = false;
            DgvUniversities.Columns[nameof(University.User)].Visible = false;

            DgvUniversities.Columns[nameof(University.UniversityName)].HeaderText = "اسم الجامعة";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(TxtUniversityName.Text))
            {
                ShowErrorMessage("يرجى كتابة اسم الجامعة");
                return;
            }

            using (UDBEntities dbContext = new UDBEntities())
            {
                AddUniversity(dbContext);
            }

            Clear();
            LoadUniversities();
        }

        private void AddUniversity(UDBEntities dbContext)
        {
            University oUniversity = new University
            {
                UniversityName = TxtUniversityName.Text,
                user_id = TUser.userID
            };

            dbContext.University.Add(oUniversity);
            dbContext.SaveChanges();
            ShowSuccessMessage("تم حفظ البيانات بنجاح", "عملية اضافة");
        }

        private bool IsTextBoxEmpty(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "خطأ!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccessMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (IsTextBoxEmpty(TxtUniversityName.Text))
            {
                ShowErrorMessage("يرجى كتابة اسم الجامعة");
                return;
            }

            if (universityID == 0)
            {
                ShowErrorMessage("يرجى اختيار جامعة بشكل صحيح من الجدول");
                return;
            }

            using (UDBEntities dbContext = new UDBEntities())
            {
                UpdateUniversity(dbContext);
            }

            Clear();
            LoadUniversities();
        }

        private void UpdateUniversity(UDBEntities dbContext)
        {
            University oUniversity = dbContext.University.FirstOrDefault(x => x.universityID == universityID);

            oUniversity.UniversityName = TxtUniversityName.Text;
            oUniversity.user_id = TUser.userID;

            dbContext.SaveChanges();
            ShowSuccessMessage("تم تعديل البيانات بنجاح", "عملية تعديل");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!ShowConfirmationDialog("أنت على وشك القيام بحذف سجل ما؟", "عملية حذف"))
                return;

            if (universityID == 0)
            {
                ShowErrorMessage("يرجى اختيار جامعة بشكل صحيح من الجدول");
                return;
            }

            using (UDBEntities dbContext = new UDBEntities())
            {
                DeleteUniversity(dbContext);
            }

            Clear();
            LoadUniversities();
        }

        private void DeleteUniversity(UDBEntities dbContext)
        {
            University oUniversity = dbContext.University.FirstOrDefault(x => x.universityID == universityID);

            dbContext.University.Remove(oUniversity);
            dbContext.SaveChanges();
            ShowSuccessMessage("تم حذف البيانات بنجاح", "عملية حذف");
        }

        private bool ShowConfirmationDialog(string message, string caption)
        {
            DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        private void DgvUniversities_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            universityID = (int)DgvUniversities.CurrentRow.Cells[nameof(University.universityID)].Value;
            TxtUniversityName.Text = DgvUniversities.CurrentRow.Cells[nameof(University.UniversityName)].Value.ToString();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // You can leave this method empty if not needed.
        }
    }
}
