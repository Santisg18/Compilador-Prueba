using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDete
{
    public class Semantico
    {
        public string EvaluarTipos(string tipodato1, string operador, string tipodato2)
        {
            string tipoError = "";
            if (tipodato1 != "" && tipodato2 != "")
            {
                if (tipodato1 == "NE" || tipodato1 == "ND" || tipodato1 == "BL")
                {
                    if (tipodato2 == "NE" || tipodato2 == "ND" || tipodato2 == "BL")
                    {

                    }
                    else
                    {
                        tipoError = "Deberia ser de tipo Entero , Flotante o Booleano";
                    }
                }
                else if (tipodato1 == "CAR")
                {
                    if (operador == "==")
                    {
                        if (tipodato2 == "CAR")
                        {

                        }
                        else
                        {
                            tipoError = "Una cadena deve ser comparada con otra cadena";
                        }
                    }
                    else
                    {
                        tipoError = "";
                    }
                }
            }
            else
            {
                tipoError = "La variable no a sido declarada";
            }
            return tipoError;
        }

        public string EvaluarInteger(string tipodato)
        {
            string error = "";
            if (tipodato == "")
            {
                error = "La variable no a sido declarada";
            }
            else
            {
                if (tipodato == "NE")
                {

                }
                else
                {
                    error = "La variable no es un entero";
                }
            }
            return error;
        }
    }
}
