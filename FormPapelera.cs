using MahaigainekoAppTransakzioak.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahaigainekoAppTransakzioak
{
    public partial class FormPapelera : Form
    {
        private readonly FormPistak _mainForm;

        private DataGridView dgvEzabatuak;
        private Button btnBerreskuratu;
        private Button btnHardDelete;

        public FormPapelera(FormPistak mainForm)
        {
            _mainForm = mainForm;

            InitializeComponentManual();

            this.Text = "Papelera - Ezabatutako Pistak";
            this.Size = new Size(600, 420);
            this.StartPosition = FormStartPosition.CenterParent;

            this.Load += FormPapelera_Load;
            btnBerreskuratu.Click += BtnBerreskuratu_Click;
            btnHardDelete.Click += BtnHardDelete_Click;
        }

        private void InitializeComponentManual()
        {
            dgvEzabatuak = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 280,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            btnBerreskuratu = new Button { Text = "Berreskuratu", Location = new Point(140, 310), Width = 140, Height = 35 };
            btnHardDelete = new Button { Text = "Betirakoko Ezabatu", Location = new Point(300, 310), Width = 140, Height = 35, BackColor = Color.LightCoral };

            this.Controls.Add(dgvEzabatuak);
            this.Controls.Add(btnBerreskuratu);
            this.Controls.Add(btnHardDelete);
        }

        private async void FormPapelera_Load(object sender, EventArgs e)
        {
            await KargatuEzabatuak();
        }

        private async Task KargatuEzabatuak()
        {
            var pistak = await ApiClient.GetPistakEzabatuakAsync();
            dgvEzabatuak.DataSource = pistak;
        }

        private async void BtnBerreskuratu_Click(object sender, EventArgs e)
        {
            if (dgvEzabatuak.CurrentRow?.DataBoundItem is PistaModel pista)
            {
                var response = await ApiClient.RestorePistaAsync(pista.Id);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"'{pista.Izena}' berriro aktibatu da.", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await KargatuEzabatuak();
                    await _mainForm.KargatuPistak();
                }
            }
        }

        private async void BtnHardDelete_Click(object sender, EventArgs e)
        {
            if (dgvEzabatuak.CurrentRow?.DataBoundItem is PistaModel pista)
            {
                var confirm = MessageBox.Show($"Ziur zaude '{pista.Izena}' betiko ezabatu nahi duzula? Ekintza hau ezin da desegin.", "KONTUZ!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    var response = await ApiClient.HardDeletePistaAsync(pista.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Pista betiko ezabatu da.", "Informazioa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await KargatuEzabatuak();
                    }
                }
            }
        }
    }
}