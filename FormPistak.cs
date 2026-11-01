using MahaigainekoAppTransakzioak.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahaigainekoAppTransakzioak
{
    public partial class FormPistak : Form
    {
        private DataGridView dgvPistak;
        private Button btnPapelera;

        public FormPistak()
        {
            InitializeComponentManual();

            this.Text = "Pisten Kudeaketa";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Load += FormPistak_Load;
            dgvPistak.CellDoubleClick += DgvPistak_CellDoubleClick;
            btnPapelera.Click += BtnPapelera_Click;
        }

        private void InitializeComponentManual()
        {
            this.dgvPistak = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 320,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.btnPapelera = new Button
            {
                Text = "Papelera",
                Size = new Size(120, 35),
                Location = new Point(250, 340)
            };

            this.Controls.Add(this.dgvPistak);
            this.Controls.Add(this.btnPapelera);
        }

        private async void FormPistak_Load(object sender, EventArgs e)
        {
            await KargatuPistak();
        }

        public async Task KargatuPistak()
        {
            try
            {
                var pistak = await ApiClient.GetPistakAktiboakAsync();

                if (pistak == null || pistak.Count == 0)
                {
                    MessageBox.Show("Zerrenda hutsik iritsi da (0 pista).", "Abisoa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    dgvPistak.DataSource = null; // Limpiar antes de asignar
                    dgvPistak.DataSource = pistak;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea gertatu da: {ex.Message}", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvPistak_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedPista = (PistaModel)dgvPistak.Rows[e.RowIndex].DataBoundItem;
                var formXehetasunak = new FormPistaXehetasunak(selectedPista, this);
                formXehetasunak.ShowDialog();
            }
        }

        private void BtnPapelera_Click(object sender, EventArgs e)
        {
            var formPapelera = new FormPapelera(this);
            formPapelera.ShowDialog();
        }
    }
}