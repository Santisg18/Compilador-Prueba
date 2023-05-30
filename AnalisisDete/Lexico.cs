using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDete
{
    public class Lexico
    {
        public string cadenaAEvaluar = string.Empty;
        int apuntadorDelantero = 0;
        int inicioLexema = 0;
        int lineaActuual = 1;
        public DataTable dtSimbolos = new DataTable();
        public DataTable dtErrores = new DataTable();
        public Lexico()
        {
            dtErrores.Columns.Add("Error" , Type.GetType("System.String"));
            dtErrores.Columns.Add("LineaError", Type.GetType("System.String"));

            dtSimbolos.Columns.Add("Token", Type.GetType("System.String"));
            dtSimbolos.Columns.Add("Lexema", Type.GetType("System.String"));
            dtSimbolos.Columns.Add("Descripcion", Type.GetType("System.String"));
            dtSimbolos.Columns.Add("Tipo", Type.GetType("System.String"));

            dtSimbolos.Rows.Add(new object[] { "NE", "NE", "Entero" });
            dtSimbolos.Rows.Add(new object[] { "ND", "ND", "Float" });
            dtSimbolos.Rows.Add(new object[] { "CAR", "CAR", "String" });
            dtSimbolos.Rows.Add(new object[] { "BL", "BL", "Boolean" });
            dtSimbolos.Rows.Add(new object[] { "VC", "VC", "MString" });
            dtSimbolos.Rows.Add(new object[] { "LEC", "LEC", "Comando lectura" });
            dtSimbolos.Rows.Add(new object[] { "ESC", "ESC", "Comando escritura" });
            dtSimbolos.Rows.Add(new object[] { "YES", "YES", "Operador if" });
            dtSimbolos.Rows.Add(new object[] { "YESNOT", "YESNOT", "Operador else" });
            dtSimbolos.Rows.Add(new object[] { "YESNOT-YES", "YESNOT-YES", "Operador else if" });
            dtSimbolos.Rows.Add(new object[] { "PR", "PR", "Operador for" });
            dtSimbolos.Rows.Add(new object[] { "MT", "MT", "Operador while" });
            dtSimbolos.Rows.Add(new object[] { "DW", "DW", "Operador do while" });
            dtSimbolos.Rows.Add(new object[] { "&", "&", "Interseccion" });
            dtSimbolos.Rows.Add(new object[] { "|", "|", "Union" });
            dtSimbolos.Rows.Add(new object[] { "!", "!", "Negacion" });
            dtSimbolos.Rows.Add(new object[] { "VE", "VE", "Vector" });
            dtSimbolos.Rows.Add(new object[] { "MA", "MA", "Matriz" });
            dtSimbolos.Rows.Add(new object[] { "ENDL", "ENDL", "Salto linea" });
            dtSimbolos.Rows.Add(new object[] { "WAKEUP", "WAKEUP", "Comando de inicio" });
            dtSimbolos.Rows.Add(new object[] { "SLEEP", "SLEEP", "Comando de fin" });
        }

        /// <summary>
        /// Obtiene el siguiente caracter de la cadena
        /// </summary>
        /// <returns>Un caracter, o EOF cuando termina</returns>
        private string ObtenerSiguienteCaracter()
        {
            string caracter = string.Empty;
            if(apuntadorDelantero < cadenaAEvaluar.Length)
            {
                caracter = cadenaAEvaluar.Substring(apuntadorDelantero, 1);
            }
            else
            {
                caracter = "EOF";
            }
            if(caracter == "\n")
            {
                lineaActuual++;
            }
            apuntadorDelantero++;
            return caracter;
        }

        private bool VerificarSiEsLetra(string caracter)
        {
            bool esLetra = false;
            if (caracter.Length == 1)
            {
                caracter = caracter.ToUpper();
                int ascci = Encoding.ASCII.GetBytes(caracter)[0];
                if(ascci>=65 && ascci<=90)
                {
                    esLetra = true;
                }
            }
            return esLetra;            
        }

        private bool VerificarSiNumero(string caracter)
        {
            bool esNumero = false;
            if (caracter.Length == 1)
            {
                int ascci = Encoding.ASCII.GetBytes(caracter)[0];
                if (ascci >= 48 && ascci <= 57)
                {
                    esNumero = true;
                }
            }
            return esNumero;
        }

        public void RegistrarTablaSimbolos(string token , string lexema, string descripcion)
        {
            if (dtSimbolos.Select("Lexema = '" + lexema + "'").Count() == 0)
            {
                dtSimbolos.Rows.Add(new object[] { token, lexema, descripcion });
            }
        }

        public void RegistrarError(string error)
        {
            dtErrores.Rows.Add(new object[] { error , lineaActuual});
        }

        private int ObtenerInicioSiguienteDiagramaEstado(int estadoActual)
        {
            int estadoSiguienteDiagrama = 0;
            switch(estadoActual)
            {
                case 0:
                    estadoSiguienteDiagrama = 3;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 3:
                    estadoSiguienteDiagrama = 8;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 8:
                    estadoSiguienteDiagrama = 16;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 16:
                    estadoSiguienteDiagrama = 21;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 21:
                    estadoSiguienteDiagrama = 24;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 24:
                    estadoSiguienteDiagrama = 28;
                    apuntadorDelantero = inicioLexema;
                    break;
                case 28:
                    estadoSiguienteDiagrama = 32;
                    apuntadorDelantero = inicioLexema;
                    break;
                default:
                    estadoSiguienteDiagrama = -1;
                    break;
            }
            return estadoSiguienteDiagrama;
        }

        public string ObtenerSiguienteComponenteLexico()
        {
            int estado = 0;
            string caracter = string.Empty;
            bool continuar = true;
            string componenteLexico = string.Empty;
            while(continuar)
            {
                switch(estado)
                {
                    case 0:
                        caracter = ObtenerSiguienteCaracter();
                        if(caracter == " " || caracter == "\n")
                        {
                            inicioLexema++;
                        }
                        else if(caracter == "EOF")
                        {
                            componenteLexico = "EOF";
                            continuar = false;
                        }
                        else if(VerificarSiEsLetra(caracter) == true)
                        {
                            estado = 1;
                        }
                        else
                        {
                            estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        }
                        break;
                    case 1:
                        caracter = ObtenerSiguienteCaracter();
                        if(VerificarSiEsLetra(caracter) == true)
                        {
                            estado = 1;
                        }
                        else
                        {
                            estado = 2;
                        }             
                        break;
                    case 2:
                        apuntadorDelantero--;
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("VBLE", componenteLexico, "Variable");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 3:
                        caracter = ObtenerSiguienteCaracter();
                        if(caracter == "+")
                        {
                            estado = 4;
                        }else if(caracter == "-")
                        {
                            estado = 5;
                        }else if(caracter == "/")
                        {
                            estado = 6;
                        }else if(caracter == "*")
                        {
                            estado = 7;
                        }
                        else if(caracter == "^")
                        {
                            estado = 26;
                        }
                        else if (caracter == "%")
                        {
                            estado = 27;
                        }
                        else
                        {
                            estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        }
                        break;
                    case 4: case 5: case 6: case 7: case 26: case 27:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("OPE_ARIT", componenteLexico, "Operador aritmetico");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 8:
                        caracter = ObtenerSiguienteCaracter();
                        if (caracter == ";" || caracter == ",") estado = 9;
                        else if (caracter == "{") estado = 10;
                        else if (caracter == "}") estado = 11;
                        else if (caracter == "(") estado = 12;
                        else if (caracter == ")") estado = 13;
                        else if (caracter == "[") estado = 14;
                        else if (caracter == "]") estado = 15;                        
                        else estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        break;
                    case 9:  
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("P_COMA", componenteLexico, "Punto y coma");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 10:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("LLA_ABRE", componenteLexico, "Abre llaves");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 11:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("LLA_CIERRA", componenteLexico, "Cierra llaves");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 12:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("PAR_ABRE", componenteLexico, "Abre parentesis");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 13:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("PAR_CIERRA", componenteLexico, "Cierra parentesis");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 14:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("COR_ABRE", componenteLexico, "Abre corchetes");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 15:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("COR_CIERRA", componenteLexico, "Cierra corchetes");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;                    
                    case 16:
                        caracter = ObtenerSiguienteCaracter();
                        if (caracter == ">") estado = 17;
                        else if (caracter == "<") estado = 18;
                        else if (caracter == "=") estado = 19;
                        else if (caracter == "!") estado = 20;                        
                        else estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        break;
                    case 17 : case 18: case 19: case 20: 
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("OPE_REL", componenteLexico, "Operador de relación");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 21:
                        caracter = ObtenerSiguienteCaracter();
                        if (caracter == ".") estado = 22;
                        else estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        break;
                    case 22:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("FIN_SEN", componenteLexico, "Final de sentencia");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 24:
                        caracter = ObtenerSiguienteCaracter();
                        int ascci = Encoding.ASCII.GetBytes(caracter)[0];
                        if (ascci == 34) estado = 25;
                        else estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        break;
                    case 25:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("COM", componenteLexico, "Comillas");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 28:
                        caracter = ObtenerSiguienteCaracter();
                        if (VerificarSiNumero(caracter) == true)
                        {
                            estado = 28;
                        }
                        else
                        {
                            estado = 29;
                        }
                        break;
                    case 29:
                        apuntadorDelantero--;
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("NUM", componenteLexico, "Numero");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case 32:
                        caracter = ObtenerSiguienteCaracter();
                        if (caracter == "&") estado = 23;
                        else if (caracter == "|") estado = 30;
                        else if (caracter == "!") estado = 31;
                        else estado = ObtenerInicioSiguienteDiagramaEstado(estado);
                        break;
                    case 23: case 30: case 31:                    
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarTablaSimbolos("OPE_LOG", componenteLexico, "Operador logico");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                    case -1:
                        componenteLexico = cadenaAEvaluar.Substring(inicioLexema, (apuntadorDelantero - inicioLexema));
                        RegistrarError(componenteLexico + " no es valido");
                        inicioLexema = apuntadorDelantero;
                        continuar = false;
                        break;
                }
            }
            return componenteLexico;
        }
    }
}
