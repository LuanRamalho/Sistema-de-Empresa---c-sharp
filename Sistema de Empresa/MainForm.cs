using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemaDeEmpresa
{
    public partial class MainForm : Form
    {
        // Controles visuais mantidos
        private Label lblNome = null!;
        private TextBox txtNome = null!;
        private Label lblFuncao = null!;
        private TextBox txtFuncao = null!;
        private Label lblSalario = null!;
        private TextBox txtSalario = null!;
        private Label lblDataAdmissao = null!;
        private TextBox txtDataAdmissao = null!;
        private Label lblDataDemissao = null!;
        private TextBox txtDataDemissao = null!;
        private Button btnAdicionar = null!;
        private Button btnEditar = null!;
        private Button btnExcluir = null!;
        private Button btnLimpar = null!;
        private GroupBox gbCadastro = null!;
        private Label lblTituloGeral = null!;
        private Label lblSímboloDólar = null!;
        private FlowLayoutPanel flpCards = null!;

        // Repositório
        private FuncionarioRepository repo = new FuncionarioRepository();

        public MainForm()
        {
            try
            {
                this.AutoScaleMode = AutoScaleMode.Dpi; 

                InitializeComponent();
                
                InicializarInterfaceColorida();
                DatabaseHelper.InitializeDatabase();
                CarregarFuncionarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro fatal ao iniciar a aplicação:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void InitializeComponent() { }

        // --- Interface Gráfica ---
        private void InicializarInterfaceColorida()
        {
            this.Text = "Gestão Profissional de Funcionários - Empresa";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 900;
            this.Height = 700;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e"); 
            this.ForeColor = Color.White; 
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point); 

            lblTituloGeral = new Label()
            {
                Text = "ADMINISTRAÇÃO DE RECURSOS HUMANOS",
                Top = 15, Left = 20, Width = 860, Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ColorTranslator.FromHtml("#4ecdc4") 
            };
            this.Controls.Add(lblTituloGeral);

            gbCadastro = new GroupBox()
            {
                Text = " Cadastro e Edição de Funcionário ",
                Top = 60, Left = 20, Width = 860, Height = 220,
                ForeColor = ColorTranslator.FromHtml("#007acc"), 
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(gbCadastro);

            Font fontLabel = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Color colorText = Color.White;

            lblNome = new Label() { Text = "Nome Completo:", Top = 40, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtNome = CriarTextBoxCustomizado(40, 180, 250);
            gbCadastro.Controls.Add(lblNome); gbCadastro.Controls.Add(txtNome);

            lblFuncao = new Label() { Text = "Função / Cargo:", Top = 80, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtFuncao = CriarTextBoxCustomizado(80, 180, 250);
            gbCadastro.Controls.Add(lblFuncao); gbCadastro.Controls.Add(txtFuncao);

            lblSalario = new Label() { Text = "Salário Base:", Top = 120, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtSalario = CriarTextBoxCustomizado(120, 180, 210, HorizontalAlignment.Right);
            lblSímboloDólar = new Label() { Text = "R$", Top = 120, Left = 395, Width = 35, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            gbCadastro.Controls.Add(lblSalario); gbCadastro.Controls.Add(txtSalario); gbCadastro.Controls.Add(lblSímboloDólar);

            lblDataAdmissao = new Label() { Text = "Data Admissão (dd/MM/yyyy):", Top = 40, Left = 460, Width = 210, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtDataAdmissao = CriarTextBoxCustomizado(40, 680, 150);
            gbCadastro.Controls.Add(lblDataAdmissao); gbCadastro.Controls.Add(txtDataAdmissao);

            lblDataDemissao = new Label() { Text = "Data Demissão (opcional):", Top = 80, Left = 460, Width = 210, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtDataDemissao = CriarTextBoxCustomizado(80, 680, 150);
            gbCadastro.Controls.Add(lblDataDemissao); gbCadastro.Controls.Add(txtDataDemissao);

            btnAdicionar = CriarBotaoModerno("✚ Adicionar", 160, 460, 120, "#4caf50");
            gbCadastro.Controls.Add(btnAdicionar);

            btnEditar = CriarBotaoModerno("✎ Editar", 160, 590, 120, "#2196f3");
            gbCadastro.Controls.Add(btnEditar);

            btnExcluir = CriarBotaoModerno("✖ Excluir", 160, 720, 120, "#f44336");
            gbCadastro.Controls.Add(btnExcluir);

            btnLimpar = CriarBotaoModerno("⟳ Limpar", 160, 330, 120, "#7f8c8d");
            gbCadastro.Controls.Add(btnLimpar);

            // Vinculando os eventos de clique aos botões
            btnLimpar.Click += (s, e) => LimparCampos();
            btnAdicionar.Click += btnAdicionar_Click;
            btnEditar.Click += btnEditar_Click;
            btnExcluir.Click += btnExcluir_Click;

            flpCards = new FlowLayoutPanel()
            {
                Top = 300,
                Left = 20,
                Width = 860,
                Height = 350,
                AutoScroll = true, 
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true 
            };
            this.Controls.Add(flpCards);

            Application.EnableVisualStyles();
        }

        private Panel CriarCardFuncionario(Funcionario f)
        {
            Panel card = new Panel()
            {
                Width = 260,
                Height = 150,
                BackColor = ColorTranslator.FromHtml("#2d2d2d"),
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = CriarPathArredondado(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 15))
                using (Pen pen = new Pen(ColorTranslator.FromHtml("#4ecdc4"), 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            Label lblNomeCard = new Label() {
                Text = f.Nome.ToUpper(),
                Top = 15, Left = 15, Width = 230,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#4ecdc4")
            };

            Label lblCargoCard = new Label() {
                Text = f.Funcao,
                Top = 45, Left = 15, Width = 230,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.White
            };

            Label lblInfo = new Label() {
                Text = $"Salário: {f.SalarioFormatado}\nAdmissão: {f.DataAdmissaoFormatada}",
                Top = 75, Left = 15, Width = 230, Height = 50,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.LightGray
            };

            // Evento de clique unificado
            EventHandler cardClick = (s, e) => SelecionarFuncionarioParaEdicao(f);
            card.Click += cardClick;
            lblNomeCard.Click += cardClick;
            lblCargoCard.Click += cardClick;
            lblInfo.Click += cardClick;

            card.Controls.Add(lblNomeCard);
            card.Controls.Add(lblCargoCard);
            card.Controls.Add(lblInfo);

            return card;
        }

        private void SelecionarFuncionarioParaEdicao(Funcionario f)
        {
            txtNome.Text = f.Nome;
            txtFuncao.Text = f.Funcao;
            txtSalario.Text = f.Salario.ToString("N2");
            txtDataAdmissao.Text = f.DataAdmissao.ToString("dd/MM/yyyy");
            txtDataDemissao.Text = f.DataDemissao.HasValue ? f.DataDemissao.Value.ToString("dd/MM/yyyy") : "";

            gbCadastro.Text = $" Editando Colaborador: {f.Nome} ";
            btnAdicionar.Enabled = false;
            
            // Armazena o funcionário atual na propriedade Tag do Form para edição/exclusão
            this.Tag = f; 
        }

        private TextBox CriarTextBoxCustomizado(int top, int left, int width, HorizontalAlignment textAlign = HorizontalAlignment.Left)
        {
            TextBox txt = new TextBox()
            {
                Top = top, Left = left, Width = width, Height = 30, 
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = ColorTranslator.FromHtml("#333333"), 
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = textAlign
            };
            return txt;
        }

        private Button CriarBotaoModerno(string text, int top, int left, int width, string hexColor)
        {
            Button btn = new Button()
            {
                Text = text, Top = top, Left = left, Width = width, Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = ColorTranslator.FromHtml(hexColor),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0; 

            btn.Paint += (s, e) =>
            {
                Button b = (Button)s!;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
                GraphicsPath path = CriarPathArredondado(rect, 10); 
                e.Graphics.FillPath(new SolidBrush(b.BackColor), path);
                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, b.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return btn;
        }

        private GraphicsPath CriarPathArredondado(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        // --- Lógica de Negócios ---
        private void CarregarFuncionarios()
        {
            flpCards.Controls.Clear();
            List<Funcionario> funcionarios = repo.GetAll();

            foreach (var f in funcionarios)
            {
                flpCards.Controls.Add(CriarCardFuncionario(f));
            }

            LimparCampos();
        }

        private void btnAdicionar_Click(object? sender, EventArgs? e)
        {
            if (ValidarCampos())
            {
                try
                {
                    var funcionario = new Funcionario
                    {
                        Nome = txtNome.Text.Trim(),
                        Funcao = txtFuncao.Text.Trim(),
                        Salario = decimal.Parse(txtSalario.Text),
                        DataAdmissao = DateTime.ParseExact(txtDataAdmissao.Text, "dd/MM/yyyy", null),
                        DataDemissao = string.IsNullOrWhiteSpace(txtDataDemissao.Text) ? (DateTime?)null : DateTime.ParseExact(txtDataDemissao.Text, "dd/MM/yyyy", null)
                    };
                    repo.Add(funcionario);
                    CarregarFuncionarios();
                    MessageBox.Show("Funcionário adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao adicionar: " + ex.Message, "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnEditar_Click(object? sender, EventArgs? e)
        {
            // Busca o funcionário que foi salvo na Tag ao clicar no card
            var funcionario = this.Tag as Funcionario;
            
            if (funcionario == null)
            {
                MessageBox.Show("Clique em um card de funcionário para editá-lo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidarCampos())
            {
                try
                {
                    funcionario.Nome = txtNome.Text.Trim();
                    funcionario.Funcao = txtFuncao.Text.Trim();
                    funcionario.Salario = decimal.Parse(txtSalario.Text);
                    funcionario.DataAdmissao = DateTime.ParseExact(txtDataAdmissao.Text, "dd/MM/yyyy", null);
                    funcionario.DataDemissao = string.IsNullOrWhiteSpace(txtDataDemissao.Text) ? (DateTime?)null : DateTime.ParseExact(txtDataDemissao.Text, "dd/MM/yyyy", null);

                    repo.Update(funcionario);
                    CarregarFuncionarios();
                    MessageBox.Show("Dados do funcionário atualizados!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao editar: " + ex.Message, "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        private void btnExcluir_Click(object? sender, EventArgs? e)
        {
            // Busca o funcionário que foi salvo na Tag ao clicar no card
            var funcionario = this.Tag as Funcionario;
            
            if (funcionario == null)
            {
                MessageBox.Show("Clique em um card de funcionário para excluí-lo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Tem certeza que deseja excluir o funcionário '{funcionario.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    repo.Delete(funcionario);
                    CarregarFuncionarios();
                    MessageBox.Show("Funcionário excluído.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtFuncao.Text = "";
            txtSalario.Text = "";
            txtDataAdmissao.Text = "";
            txtDataDemissao.Text = "";
            gbCadastro.Text = " Cadastro de Novo Funcionário "; 
            btnAdicionar.Enabled = true; 
            
            // Limpa a referência do funcionário selecionado
            this.Tag = null; 
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text)) { MostrarAviso("Nome é obrigatório."); return false; }
            if (string.IsNullOrWhiteSpace(txtFuncao.Text)) { MostrarAviso("Função é obrigatória."); return false; }
            if (!decimal.TryParse(txtSalario.Text, out _)) { MostrarAviso("Salário inválido (use apenas números)."); return false; }
            if (!DateTime.TryParseExact(txtDataAdmissao.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _)) { MostrarAviso("Data de Admissão inválida ou formato incorreto (dd/MM/yyyy)."); return false; }
            
            if (!string.IsNullOrWhiteSpace(txtDataDemissao.Text))
            {
                if (!DateTime.TryParseExact(txtDataDemissao.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _)) { MostrarAviso("Data de Demissão inválida (dd/MM/yyyy)."); return false; }
            }
            return true;
        }

        private void MostrarAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
