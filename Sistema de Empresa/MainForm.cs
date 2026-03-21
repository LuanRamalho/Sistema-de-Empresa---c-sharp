using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SistemaDeEmpresa
{
    public partial class MainForm : Form
    {
        // Controles visuais
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
        private DataGridView dgvFuncionarios = null!;

        // Repositório
        private FuncionarioRepository repo = new FuncionarioRepository();

        public MainForm()
        {
            try
            {
                // Configuração básica do formulário antes de inicializar componentes
                this.AutoScaleMode = AutoScaleMode.Dpi; // Melhora visual em telas de alta resolução

                InitializeComponent();
                // Ocultar os componentes visuais padrão gerados (se houver) para usar os personalizados
                
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

        private void InitializeComponent()
        {
            // Este método é padrão do WinForms e pode ser mantido vazio ou
            // gerado pelo designer. Toda a nossa customização está em
            // InicializarInterfaceColorida().
        }

        // --- Nova Interface Gráfica Colorida e Moderna ---
        private void InicializarInterfaceColorida()
        {
            // --- Configurações do Formulário Principal (Form) ---
            this.Text = "Gestão Profissional de Funcionários - Empresa";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 900;
            this.Height = 700;
            this.BackColor = ColorTranslator.FromHtml("#1e1e1e"); // Fundo cinza escuro moderno
            this.ForeColor = Color.White; // Texto padrão branco
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point); // Fonte mais moderna

            // --- Título Principal ---
            lblTituloGeral = new Label()
            {
                Text = "ADMINISTRAÇÃO DE RECURSOS HUMANOS",
                Top = 15,
                Left = 20,
                Width = 860,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ColorTranslator.FromHtml("#4ecdc4") // Verde menta vibrante
            };
            this.Controls.Add(lblTituloGeral);

            // --- Painel de Cadastro (GroupBox) ---
            gbCadastro = new GroupBox()
            {
                Text = " Cadastro e Edição de Funcionário ",
                Top = 60,
                Left = 20,
                Width = 860,
                Height = 220,
                ForeColor = ColorTranslator.FromHtml("#007acc"), // Azul vibrante para o título do grupo
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(gbCadastro);

            // Estilo padrão para Labels e TextBoxes dentro do GroupBox
            Font fontLabel = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Color colorText = Color.White;
            Color colorFocus = ColorTranslator.FromHtml("#333333"); // Cinza escuro para foco

            // --- Coluna 1 do Cadastro ---
            // Nome
            lblNome = new Label() { Text = "Nome Completo:", Top = 40, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtNome = CriarTextBoxCustomizado(40, 180, 250);
            gbCadastro.Controls.Add(lblNome); gbCadastro.Controls.Add(txtNome);

            // Função
            lblFuncao = new Label() { Text = "Função / Cargo:", Top = 80, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtFuncao = CriarTextBoxCustomizado(80, 180, 250);
            gbCadastro.Controls.Add(lblFuncao); gbCadastro.Controls.Add(txtFuncao);

            // Salário
            lblSalario = new Label() { Text = "Salário Base:", Top = 120, Left = 20, Width = 150, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtSalario = CriarTextBoxCustomizado(120, 180, 210, HorizontalAlignment.Right);
            lblSímboloDólar = new Label() { Text = "R$", Top = 120, Left = 395, Width = 35, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            gbCadastro.Controls.Add(lblSalario); gbCadastro.Controls.Add(txtSalario); gbCadastro.Controls.Add(lblSímboloDólar);

            // --- Coluna 2 do Cadastro ---
            // Data Admissão
            lblDataAdmissao = new Label() { Text = "Data Admissão (dd/MM/yyyy):", Top = 40, Left = 460, Width = 210, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtDataAdmissao = CriarTextBoxCustomizado(40, 680, 150);
            gbCadastro.Controls.Add(lblDataAdmissao); gbCadastro.Controls.Add(txtDataAdmissao);

            // Data Demissão
            lblDataDemissao = new Label() { Text = "Data Demissão (opcional):", Top = 80, Left = 460, Width = 210, Font = fontLabel, ForeColor = colorText, TextAlign = ContentAlignment.MiddleLeft };
            txtDataDemissao = CriarTextBoxCustomizado(80, 680, 150);
            gbCadastro.Controls.Add(lblDataDemissao); gbCadastro.Controls.Add(txtDataDemissao);

            // --- Botões com Visual Moderno ---
            // Botão Adicionar (Verde)
            btnAdicionar = CriarBotaoModerno("✚ Adicionar", 160, 460, 120, "#4caf50");
            gbCadastro.Controls.Add(btnAdicionar);

            // Botão Editar (Azul)
            btnEditar = CriarBotaoModerno("✎ Editar", 160, 590, 120, "#2196f3");
            gbCadastro.Controls.Add(btnEditar);

            // Botão Excluir (Vermelho)
            btnExcluir = CriarBotaoModerno("✖ Excluir", 160, 720, 120, "#f44336");
            gbCadastro.Controls.Add(btnExcluir);

            // Botão Limpar (Cinza/Laranja)
            btnLimpar = CriarBotaoModerno("⟳ Limpar", 160, 330, 120, "#7f8c8d");
            gbCadastro.Controls.Add(btnLimpar);

            // Para limpar os campos
            btnLimpar.Click += (s, e) => LimparCampos();

            // --- DataGridView com Estilo Escuro e Colorido ---
            dgvFuncionarios = new DataGridView()
            {
                Top = 300,
                Left = 20,
                Width = 860,
                Height = 350,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                MultiSelect = false,
                BackgroundColor = ColorTranslator.FromHtml("#2d2d2d"), // Fundo da tabela
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false, // Permite customizar cabeçalho
                GridColor = ColorTranslator.FromHtml("#444444"), // Cor das linhas da grade
                RowHeadersVisible = false // Esconde a coluna de seta à esquerda
            };

            // Estilo do Cabeçalho
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#007acc"); // Azul vibrante
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvFuncionarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFuncionarios.ColumnHeadersHeight = 40;

            // Estilo das Linhas
            dgvFuncionarios.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2d2d2d");
            dgvFuncionarios.DefaultCellStyle.ForeColor = Color.White;
            dgvFuncionarios.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvFuncionarios.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#4ecdc4"); // Verde menta para seleção
            dgvFuncionarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvFuncionarios.RowTemplate.Height = 35; // Linhas mais altas

            this.Controls.Add(dgvFuncionarios);

            // --- Assinatura de Eventos ---
            btnAdicionar.Click += btnAdicionar_Click;
            btnEditar.Click += btnEditar_Click;
            btnExcluir.Click += btnExcluir_Click;
            dgvFuncionarios.SelectionChanged += dgvFuncionarios_SelectionChanged;

            // Configuração para aplicar o tema em outros controles (como MessageBoxes)
            Application.EnableVisualStyles();
        }

        // Método auxiliar para criar TextBoxes com estilo padronizado
        private TextBox CriarTextBoxCustomizado(int top, int left, int width, HorizontalAlignment textAlign = HorizontalAlignment.Left)
        {
            TextBox txt = new TextBox()
            {
                Top = top,
                Left = left,
                Width = width,
                Height = 30, // Um pouco mais alto
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = ColorTranslator.FromHtml("#333333"), // Cinza escuro
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = textAlign
            };
            return txt;
        }

        // Método auxiliar para criar botões modernos com cores customizadas
        private Button CriarBotaoModerno(string text, int top, int left, int width, string hexColor)
        {
            Button btn = new Button()
            {
                Text = text,
                Top = top,
                Left = left,
                Width = width,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = ColorTranslator.FromHtml(hexColor),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0; // Sem borda padrão

            // Adiciona efeito de borda arredondada via pintura customizada
            btn.Paint += (s, e) =>
            {
                Button b = (Button)s!;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
                GraphicsPath path = CriarPathArredondado(rect, 10); // Raio de 10px
                e.Graphics.FillPath(new SolidBrush(b.BackColor), path);
                
                // Desenha o texto centralizado
                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, b.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return btn;
        }

        // Método auxiliar para gerar o caminho gráfico para cantos arredondados
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

        // --- Lógica de Negócios (Mantida e refatorada levemente) ---
        private void CarregarFuncionarios()
        {
            dgvFuncionarios.DataSource = null;
            List<Funcionario> funcionarios = repo.GetAll();
            dgvFuncionarios.DataSource = funcionarios;

            // Esconde colunas técnicas (ID e valores brutos não formatados)
            OcultarColunaSeExistir("Id");
            OcultarColunaSeExistir("Salario");
            OcultarColunaSeExistir("DataAdmissao");
            OcultarColunaSeExistir("DataDemissao");

            // Define títulos amigáveis e centraliza dados numéricos/datas
            ConfigurarColunaSeExistir("Nome", "Nome do Colaborador", HorizontalAlignment.Left);
            ConfigurarColunaSeExistir("Funcao", "Função / Cargo", HorizontalAlignment.Left);
            ConfigurarColunaSeExistir("SalarioFormatado", "Salário (R$)", HorizontalAlignment.Right, 2);
            ConfigurarColunaSeExistir("DataAdmissaoFormatada", "Admissão", HorizontalAlignment.Center, 4);
            ConfigurarColunaSeExistir("DataDemissaoFormatada", "Demissão", HorizontalAlignment.Center, 5);

            dgvFuncionarios.ClearSelection(); // Evita selecionar o primeiro item ao abrir
            LimparCampos();
        }

        private void OcultarColunaSeExistir(string nomeColuna)
        {
            if (dgvFuncionarios.Columns.Contains(nomeColuna))
                dgvFuncionarios.Columns[nomeColuna].Visible = false;
        }

        private void ConfigurarColunaSeExistir(string nomeColuna, string titulo, HorizontalAlignment alinhamento, int displayIndex = -1)
        {
            if (dgvFuncionarios.Columns.Contains(nomeColuna))
            {
                var col = dgvFuncionarios.Columns[nomeColuna];
                col.HeaderText = titulo;
                if (displayIndex != -1) col.DisplayIndex = displayIndex;
                
                switch (alinhamento)
                {
                    case HorizontalAlignment.Left: col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; break;
                    case HorizontalAlignment.Right: col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; break;
                    case HorizontalAlignment.Center: col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; break;
                }
            }
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
                    LimparCampos();
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
            if (dgvFuncionarios.CurrentRow == null)
            {
                MessageBox.Show("Selecione um funcionário na tabela para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var funcionario = dgvFuncionarios.CurrentRow.DataBoundItem as Funcionario;
            if (funcionario == null) return;

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
                    LimparCampos();
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
            if (dgvFuncionarios.CurrentRow == null)
            {
                MessageBox.Show("Selecione um funcionário na tabela para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var funcionario = dgvFuncionarios.CurrentRow.DataBoundItem as Funcionario;
            if (funcionario == null) return;

            if (MessageBox.Show($"Tem certeza que deseja excluir o funcionário '{funcionario.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    repo.Delete(funcionario.Id);
                    CarregarFuncionarios();
                    LimparCampos();
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
            gbCadastro.Text = " Cadastro de Novo Funcionário "; // Reseta o título do grupo
            btnAdicionar.Enabled = true; // Reativa o botão adicionar
            dgvFuncionarios.ClearSelection(); // Desmarca a linha na tabela
        }

        // Validação simples dos campos antes de enviar para o repositório
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

        private void dgvFuncionarios_SelectionChanged(object? sender, EventArgs? e)
        {
            // Verifica se há uma linha selecionada e se o foco está na tabela (clique real)
            if (dgvFuncionarios.CurrentRow == null || !dgvFuncionarios.Focused) return;
            
            var funcionario = dgvFuncionarios.CurrentRow.DataBoundItem as Funcionario;
            if (funcionario == null) return;

            // Preenche os campos para edição
            txtNome.Text = funcionario.Nome;
            txtFuncao.Text = funcionario.Funcao;
            txtSalario.Text = funcionario.Salario.ToString("N2");
            txtDataAdmissao.Text = funcionario.DataAdmissao.ToString("dd/MM/yyyy");
            txtDataDemissao.Text = funcionario.DataDemissao.HasValue ? funcionario.DataDemissao.Value.ToString("dd/MM/yyyy") : "";

            // Atualiza o visual para indicar modo de edição
            gbCadastro.Text = $" Editando Colaborador: {funcionario.Nome} ";
            btnAdicionar.Enabled = false; 
        }
    }
}