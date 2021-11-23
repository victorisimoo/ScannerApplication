using System;
using System.IO;
using System.Windows.Forms;
using ParserApplication.Structure;
using ParserApplication.TokenConstruction;
using System.Collections.Generic;
using ParserApplication.LALR;

namespace ParserApplication {
    public partial class Parser : Form {
        public Parser() {
            InitializeComponent();
            txtFile.Enabled = false;
        }

        public string gramatica = "";
        private void btnFileSelect_Click(object sender, EventArgs e) {
            var openFileDialog = new OpenFileDialog() {
                Filter = "Text files (*.y)|*.y",
                Title = "ArchivoSeleccionado"
            };

            try {
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    var filePath = openFileDialog.FileName;
                    var fileName = Path.GetFileName(filePath);
                    DialogResult dialogResult = MessageBox.Show($"¿Desea analizar la gramática que se encuentra en {fileName}?", "Confirmar análisis", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) {
                        txtFile.Text = filePath;
                        txtFile.Enabled = false;
                        grammarAnalyzer(openFileDialog.OpenFile(), fileName);
                    }

                }
            }
            catch {

                txtResult.ForeColor = System.Drawing.Color.Red;
                txtResult.Text = "ERROR EN LA GRAMÁTICA: " + getGramatica();
                MessageBox.Show("Se han encontrado un error en el análisis de la gramática",
                        "¡Error encontrado!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grammarAnalyzer(Stream fileStream, string fileName) {
            using (StreamReader reader = new StreamReader(fileStream)) {
                var line = reader.ReadLine();
                string gramaticaCompleta = line;
                while ((line = reader.ReadLine()) != null) {
                    gramaticaCompleta = gramaticaCompleta + line;
                }
                gramatica = gramaticaCompleta;
                labelmostrar.Text = gramaticaCompleta.Replace(";", "\r\n");
            }

            Queue<Token> entrada = new Queue<Token>();
            Scanner scanner = new Scanner(gramatica);
            Token nextToken;

            do {
                nextToken = scanner.GetToken();
                entrada.Enqueue(nextToken);
            } while (nextToken.Tag != TokenType.EOF);

            if (scanner.getErrorResult()) {
                txtResult.ForeColor = System.Drawing.Color.Red;
                txtResult.Text = "ERROR EN LA GRAMÁTICA: " + getGramatica();

            } else {
                txtResult.ForeColor = System.Drawing.Color.Green;
                txtResult.Text = "La gramática se ha validado correctamente: \r\n" + gramatica.Replace(";", "\r\n");

            }
            Token[] entradas = entrada.ToArray();
            //Lexico todo bien
            Parsers parser = new Parsers(entrada);
            parser.Parse2();
            //parser todo bien
            // ingresar al lalr
            Rules concatenartokens = new Rules(entradas);
            Graph Grafo = new Graph(concatenartokens.Reglas);
            Grafo.BuildGraph();
            
        }

        public string getGramatica() {
            return this.gramatica;
        }

        private void btnAnalysis_Click(object sender, EventArgs e) {
            string value = "'" + txtAnalysis.Text + "'";
            value = value.Replace(" ", "' '");
            Queue<Token> entrada = new Queue<Token>();
            Scanner scanner = new Scanner(value);
            Token nextToken;
            do
            {
                nextToken = scanner.GetToken();
                entrada.Enqueue(nextToken);
            } while (nextToken.Tag != TokenType.EOF);
            //EN ENTRADA SE ENCUENTRAN LOS TOKENS PARA PARSEO
            if (scanner.getErrorResult())
            {

                lblAnalysisResult.ForeColor = System.Drawing.Color.Red;
                lblAnalysisResult.Text = "INCORRECTO";
            }
            else
            {
                lblAnalysisResult.ForeColor = System.Drawing.Color.Green;
                lblAnalysisResult.Text = "CORRECTO";
            }

        }


        private void txtResult_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Parser_Load(object sender, EventArgs e)
        {

        }
    }
}
