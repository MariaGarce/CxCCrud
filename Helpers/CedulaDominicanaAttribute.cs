using System.ComponentModel.DataAnnotations;

public class CedulaDominicanaAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return false;
        }

        string cedula = value.ToString();
        return ValidaCedula(cedula);
    }

    private bool ValidaCedula(string cedula)
    {
        int verificador, digito, digitoVerificador, digitoImpar;
        int sumaPar = 0, sumaImpar = 0;

        if (string.IsNullOrWhiteSpace(cedula) || cedula.Length != 11 || !cedula.All(char.IsDigit))
        {
            return false;
        }

        try
        {
            digitoVerificador = Convert.ToInt32(cedula.Substring(10, 1));

            for (int i = 9; i >= 0; i--)
            {
                digito = Convert.ToInt32(cedula.Substring(i, 1));

                if ((i % 2) != 0) // Posiciones impares (según el índice 0)
                {
                    digitoImpar = digito * 2;
                    if (digitoImpar >= 10)
                    {
                        digitoImpar -= 9;
                    }
                    sumaImpar += digitoImpar;
                }
                else
                {
                    sumaPar += digito;
                }
            }

            verificador = 10 - ((sumaPar + sumaImpar) % 10);
            if (verificador == 10)
            {
                verificador = 0;
            }

            return verificador == digitoVerificador;
        }
        catch
        {
            return false;
        }
    }
}
