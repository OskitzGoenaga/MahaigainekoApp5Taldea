using MahaigainekoAppTransakzioak.Dto;
using MahaigainekoAppTransakzioak.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahaigainekoAppTransakzioak
{
    public partial class FormPistaXehetasunak : Form
    {
        private readonly PistaModel _pista;
        private readonly FormPistak _parentForm;

        // Elementos UI
        private Label lblPistaIzena;
        private ComboBox cmbBazkideak;
        private DateTimePicker dtpData;
        private TextBox txtOrdua;
        private Button btnErreserbatu;
        private Button btnEzabatu;

        public FormPistaXehetasunak(PistaModel pista, FormPistak parentForm)
        {
            _pista = pista;
            _parentForm = parentForm;

            InitializeComponentManual();

            this.Text = $"Pista: {_pista.Izena}";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterParent;

            this.Load += FormPistaXehetasunak_Load;
            btnErreserbatu.Click += BtnErreserbatu_Click;
            btnEzabatu.Click += BtnEzabatu_Click;
        }

        private void InitializeComponentManual()
        {
            lblPistaIzena = new Label { Location = new Point(20, 20), AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold) };

            var lblBazkidea = new Label { Text = "Bazkidea:", Location = new Point(20, 60), AutoSize = true };
            cmbBazkideak = new ComboBox { Location = new Point(120, 55), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblData = new Label { Text = "Data:", Location = new Point(20, 100), AutoSize = true };
            dtpData = new DateTimePicker { Location = new Point(120, 95), Width = 220, Format = DateTimePickerFormat.Short };

            var lblOrdua = new Label { Text = "Ordua:", Location = new Point(20, 140), AutoSize = true };
            txtOrdua = new TextBox { Location = new Point(120, 135), Width = 220, Text = "18:00:00" };

            btnErreserbatu = new Button { Text = "Erreserba Egin", Location = new Point(20, 190), Width = 160, Height = 35, BackColor = Color.LightGreen };
            btnEzabatu = new Button { Text = "Pista Desaktibatu", Location = new Point(190, 190), Width = 150, Height = 35, BackColor = Color.LightCoral };

            this.Controls.Add(lblPistaIzena);
            this.Controls.Add(lblBazkidea);
            this.Controls.Add(cmbBazkideak);
            this.Controls.Add(lblData);
            this.Controls.Add(dtpData);
            this.Controls.Add(lblOrdua);
            this.Controls.Add(txtOrdua);
            this.Controls.Add(btnErreserbatu);
            this.Controls.Add(btnEzabatu);
        }

        private async void FormPistaXehetasunak_Load(object sender, EventArgs e)
        {
            lblPistaIzena.Text = $"{_pista.Izena} ({_pista.Kirola})";

            var bazkideak = await ApiClient.GetBazkideakAsync();
            cmbBazkideak.DataSource = bazkideak;
            cmbBazkideak.DisplayMember = "IzenaOsoa";
            cmbBazkideak.ValueMember = "Id";
        }

        private async void BtnErreserbatu_Click(object sender, EventArgs e)
        {
            if (cmbBazkideak.SelectedValue == null) return;

            var dto = new ErreserbaCreateDto
            {
                PistaId = _pista.Id,
                BazkideaId = (int)cmbBazkideak.SelectedValue,
                Data = dtpData.Value.ToString("yyyy-MM-dd"),
                Ordua = txtOrdua.Text
            };

            var response = await ApiClient.PostErreserbaAsync(dto);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Erreserba egoki exekutatu da!", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Ezin izan da erreserba egin (dagoeneko hartuta egon daiteke).", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEzabatu_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show($"Ziur zaude '{_pista.Izena}' pista desaktibatu nahi duzula?", "Baieztatu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var response = await ApiClient.SoftDeletePistaAsync(_pista.Id);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Pista papelerara bidali da.", "Informazioa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar las pistas en el formulario principal para que desaparezca la fila borrada
                    await _parentForm.KargatuPistak();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ezin izan da pista desaktibatu.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}