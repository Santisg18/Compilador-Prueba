using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalisisDete
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Lexico objLexico = new Lexico();
        Semantico objSemantico = new Semantico();
        string preanalisis = string.Empty;
        private void btnCompilar_Click(object sender, EventArgs e)
        {
            objLexico = new Lexico();
            objLexico.cadenaAEvaluar = txtAlgoritmo.Text;
            string compLexico = string.Empty;
            do
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();

                switch (ObtenerTipoToken(preanalisis))
                {
                    case "NE":
                    case "ND":
                    case "CAR":
                    case "BL":
                        Declaracion_de_variable();
                        break;
                    case "ESC":
                        Comando_de_escritura();
                        break;
                    case "LEC":
                        Comando_de_lectura();
                        break;
                    case "VE":
                        Declaracion_de_vector();
                        break;
                    case "MA":
                        Declaracion_de_matriz();
                        break;
                    case "YES":
                        Estructura_if();
                        break;
                    case "YESNOT":
                        Estructura_else();
                        break;
                    case "MT":
                        Estructura_repetitiva();
                        break;
                    case "VBLE":
                        Estructura_secuencial();
                        break;
                    case "WAKEUP":
                        Comando_inicio_y_fin();
                        break;
                }

            } while (preanalisis != "EOF");

            dgTablaSimbolos.DataSource = objLexico.dtSimbolos;
            dgErrores.DataSource = objLexico.dtErrores;
        }

        private void Comando_inicio_y_fin()
        {
            Evaluar_comando_inicio();
            evaluar_expresiones();
            Evaluar_comando_fin();
        }

        private void Estructura_secuencial()
        {            
            EvaluarVariable();
            EvaluarIgual();
            EvaluarOperando();
            EvaluarResto_secuencial();
            Evaluar_punto();
        }

        private void Estructura_repetitiva()
        {            
            Evaluar_estructura_while();
            Evaluar_parentesisA();
            EvaluarOperando();
            EvaluarOperadorRelacionable();
            EvaluarOperando();
            EvaluarResto_if();
            Evaluar_parentesisC();
            Evaluar_LlaA();
            evaluar_expresiones();
            Evaluar_LlaC();
        }

        private void Estructura_else()
        {            
            Evaluar_estructura_else();
            Evaluar_parentesisA();
            Evaluar_parentesisC();
            Evaluar_LlaA();
            evaluar_expresiones();
            Evaluar_LlaC();
        }

        private void Estructura_if()
        {            
            Evaluar_estructura_if();
            Evaluar_parentesisA();
            EvaluarOperando();
            EvaluarOperadorRelacionable();
            EvaluarOperando();
            EvaluarResto_if();
            Evaluar_parentesisC();
            Evaluar_LlaA();
            evaluar_expresiones();
            Evaluar_LlaC();
        }

        private void Declaracion_de_matriz()
        {            
            Evaluar_declaracion_de_matriz();
            EvaluarVariable();
            Evaluar_parentesisA();
            EvaluarOperando();
            Evaluar_parentesisC();
            Evaluar_parentesisA();
            EvaluarOperando();
            Evaluar_parentesisC();
            Evaluar_punto();
        }

        private void Declaracion_de_vector()
        {            
            Evaluar_declaracion_de_vector();
            EvaluarVariable();
            Evaluar_parentesisA();
            EvaluarOperando();
            Evaluar_parentesisC();
            Evaluar_punto();
        }

        private void Comando_de_lectura()
        {            ;
            Evaluar_comando_de_lectura();
            EvaluarVariable();
            Evaluar_punto();
        }

        private void Comando_de_escritura()
        {            
            Evaluar_comando_escritura();
            Evaluar_parentesisA();
            Evaluar_comillas();
            Evaluar_texto();
            Evaluar_comillas();
            Evaluar_restoComandoEscritura();
            Evaluar_parentesisC();
            Evaluar_punto();
        }

        private void Evaluar_comando_inicio()
        {
            if (ObtenerTipoToken(preanalisis) == "WAKEUP")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba comando inicio");
            }
        }

        private void Evaluar_comando_fin()
        {
            if (ObtenerTipoToken(preanalisis) == "SLEEP")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba comando fin");
            }
        }

        private void Evaluar_estructura_while()
        {
            if (ObtenerTipoToken(preanalisis) == "MT")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void Evaluar_estructura_else()
        {
            if (ObtenerTipoToken(preanalisis) == "YESNOT")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void Evaluar_estructura_if()
        {
            if (ObtenerTipoToken(preanalisis) == "YES")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void EvaluarResto_if()
        {
            if (ObtenerTipoToken(preanalisis) == "OPE_LOG")
            {
                EvaluarOperadorLogico();
                EvaluarOperando();
                EvaluarOperadorRelacionable();
                EvaluarOperando();
                EvaluarResto_if();
            }
        }

        private void EvaluarOperadorLogico()
        {
            if (ObtenerTipoToken(preanalisis) == "OPE_LOG")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba operador logico");
            }
        }

        public void evaluar_expresiones()
        {
            if (ObtenerTipoToken(preanalisis) == "NE" || ObtenerTipoToken(preanalisis) == "ND" || ObtenerTipoToken(preanalisis) == "CAR" || ObtenerTipoToken(preanalisis) == "BL"
            || ObtenerTipoToken(preanalisis) == "VBLE" || ObtenerTipoToken(preanalisis) == "MT" || ObtenerTipoToken(preanalisis) == "YESNOT" || ObtenerTipoToken(preanalisis) == "YES" || ObtenerTipoToken(preanalisis) == "MA"
             || ObtenerTipoToken(preanalisis) == "VE" || ObtenerTipoToken(preanalisis) == "ESC" || ObtenerTipoToken(preanalisis) == "LEC")
            {
                expresiones();
                evaluar_expresiones();
            }
        }

        private void expresiones()
        {
            if (ObtenerTipoToken(preanalisis) == "VBLE")
            {
                Estructura_secuencial();
            }
            else
            {
                if (ObtenerTipoToken(preanalisis) == "MT")
                {
                    Estructura_repetitiva();
                }
                else
                {
                    if (ObtenerTipoToken(preanalisis) == "YESNOT")
                    {
                        Estructura_else();
                    }
                    else
                    {
                        if (ObtenerTipoToken(preanalisis) == "YES")
                        {
                            Estructura_if();
                        }
                        else
                        {
                            if (ObtenerTipoToken(preanalisis) == "MA")
                            {
                                Declaracion_de_matriz();
                            }
                            else
                            {
                                if (ObtenerTipoToken(preanalisis) == "VE")
                                {
                                    Declaracion_de_vector();
                                }
                                else
                                {
                                    if (ObtenerTipoToken(preanalisis) == "ESC")
                                    {
                                        Comando_de_escritura();
                                    }
                                    else
                                    {
                                        if (ObtenerTipoToken(preanalisis) == "LEC")
                                        {
                                            Comando_de_lectura();
                                        }
                                        else
                                        {
                                            if (ObtenerTipoToken(preanalisis) == "NE" || ObtenerTipoToken(preanalisis) == "ND" || ObtenerTipoToken(preanalisis) == "CAR" || ObtenerTipoToken(preanalisis) == "BL")
                                            {
                                                Declaracion_de_variable();
                                            }
                                            else

                                                objLexico.RegistrarError("Se esperaba una expresion");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EvaluarOperadorRelacionable()
        {
            if (ObtenerTipoToken(preanalisis) == "OPE_REL")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba operador relacionable");
            }
        }

        private void Evaluar_declaracion_de_matriz()
        {
            if (ObtenerTipoToken(preanalisis) == "MA")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void EvaluarOperando()
        {
            if (ObtenerTipoToken(preanalisis) == "VBLE")
            {
                EvaluarVariable();
            }
            else
            if (ObtenerTipoToken(preanalisis) == "NUM")
                EvaluarNumero();
            else
            {
                objLexico.RegistrarError("Se esperaba operando");
            }
        }

        private void EvaluarNumero()
        {
            if (ObtenerTipoToken(preanalisis) == "NUM")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba numero");
            }
        }

        private void EvaluarIgual()
        {
            if (preanalisis == "=")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba igual");
            }
        }

        private void Evaluar_punto()
        {
            if (preanalisis == ".")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba un punto");
            }
        }

        private void Evaluar_declaracion_de_vector()
        {
            if (ObtenerTipoToken(preanalisis) == "VE")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }

        }

        private void Evaluar_comando_de_lectura()
        {
            if (ObtenerTipoToken(preanalisis) == "LEC")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }

        }

        private void Evaluar_LlaA()
        {
            if (preanalisis == "{")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba llave");
            }
        }

        private void Evaluar_LlaC()
        {
            if (preanalisis == "}")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba llave");
            }
        }

        private void Evaluar_parentesisA()
        {
            if (preanalisis == "(")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba parentesis");
            }
        }        

        private void Evaluar_restoComandoEscritura()
        {
            if (preanalisis == "+")
            {
                EvaluarMas();
                Evaluar_comillas();
                Evaluar_texto();
                Evaluar_comillas();
                Evaluar_restoComandoEscritura();
            }
        }

        private void Evaluar_texto()
        {
            if (ObtenerTipoToken(preanalisis) == "VBLE" || ObtenerTipoToken(preanalisis) == "NUM" || ObtenerTipoToken(preanalisis) == "OPE_LOG" || ObtenerTipoToken(preanalisis) == "OPE_REL" || ObtenerTipoToken(preanalisis) == "OPE_ARIT")
            {
                EvaluarOtrol();
                Evaluar_texto();
            }
        }

        private void EvaluarOtrol()
        {
            if (ObtenerTipoToken(preanalisis) == "VBLE")
            {
                EvaluarVariable();
            }
            else
           if (ObtenerTipoToken(preanalisis) == "NUM")
                EvaluarNumero();
            else
            {
                if (ObtenerTipoToken(preanalisis) == "OPE_LOG")
                {
                    EvaluarOperadorLogico();
                }
                else
                {
                    if (ObtenerTipoToken(preanalisis) == "OPE_REL")
                    {
                        EvaluarOperadorRelacionable();
                    }
                    else
                    {
                        if (ObtenerTipoToken(preanalisis) == "OPE_ARIT")
                        {
                            EvaluarOperadorAritmetico();
                        }
                        else
                        {
                            objLexico.RegistrarError("Se esperaba un texto o vble");
                        }
                    }
                }
            }
        }

        private void Evaluar_comillas()
        {
            int ascci = Encoding.ASCII.GetBytes(preanalisis)[0];
            if (ascci == 34)
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaban comillas");
            }
        }

        private void Evaluar_parentesisC()
        {
            if (preanalisis == ")")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba parentesis");
            }
        }

        private void Evaluar_resto()
        {
            EvaluarVariable();
            Evaluar_resto();
        }

        private void Evaluar_comando_escritura()
        {
            if (ObtenerTipoToken(preanalisis) == "ESC")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void Declaracion_de_variable()
        {
            EvaluarTipo();
            EvaluarVariable();
            Evaluar_punto();
        }

        private void EvaluarTipo()
        {
            if (ObtenerTipoToken(preanalisis) == "NE" || ObtenerTipoToken(preanalisis) == "ND" || ObtenerTipoToken(preanalisis) == "CAR" || ObtenerTipoToken(preanalisis) == "BL")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
        }

        private void EvaluarVariable()
        {
            if (ObtenerTipoToken(preanalisis) == "VBLE")
            {                            
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }            
            else
            {
                objLexico.RegistrarError("Se esperaba una variable");
            }
        }

        private void EvaluarResto_secuencial()
        {
            if (ObtenerTipoToken(preanalisis) == "OPE_ARIT")
            {
                EvaluarOperadorAritmetico();
                EvaluarOperando();
                EvaluarResto_secuencial();
            }
            else
            {
                if (preanalisis == "+")
                {
                    EvaluarMas();
                    EvaluarOperando();
                    EvaluarResto_secuencial();
                }
            }
        }

        private void EvaluarMas()
        {
            if (preanalisis == "+")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba un mas");
            }
        }

        private void EvaluarOperadorAritmetico()
        {
            if (ObtenerTipoToken(preanalisis) == "OPE_ARIT")
            {
                preanalisis = objLexico.ObtenerSiguienteComponenteLexico();
            }
            else
            {
                objLexico.RegistrarError("Se esperaba operador aritmetico");
            }
        }

        private string ObtenerTipoToken(string lexema)
        {
            string tipoToken = string.Empty;
            DataRow[] elementos = objLexico.dtSimbolos.Select("Lexema = '" + lexema + "'");
            if (elementos.Count() > 0)
            {
                tipoToken = elementos[0]["Token"].ToString();
            }
            return tipoToken;
        }

        private string RegistrarTipoDato(string tipo, string vble)
        {
            string tipodato = string.Empty;
            DataRow[] dr = objLexico.dtSimbolos.Select("Lexema='" + vble + "'");
            if (dr.Length > 0)
            {
                dr[0]["TipoDato"] = tipo;
            }
            return tipodato;
        }

        private void txtAlgoritmo_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgTablaSimbolos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgErrores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
