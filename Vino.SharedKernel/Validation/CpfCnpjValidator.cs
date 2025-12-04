namespace BetterThanYou.SharedKernel.Validation;

public static class CpfCnpjValidator
{
    public static string? RemoverCaracteresEspeciais(string? cpfCnpj)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return null;
        
        return new string(cpfCnpj.Where(char.IsDigit).ToArray());
    }
    
    public static bool ValidarCpfCnpj(string? cpfCnpj)
    {
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return true; // CPF/CNPJ é opcional
        
        cpfCnpj = RemoverCaracteresEspeciais(cpfCnpj);
        
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            return false;
        
        return cpfCnpj.Length == 11 ? ValidarCpf(cpfCnpj) : cpfCnpj.Length == 14 && ValidarCnpj(cpfCnpj);
    }
    
    private static bool ValidarCpf(string cpf)
    {
        if (cpf.Length != 11)
            return false;
        
        // Verifica se todos os dígitos são iguais
        if (cpf.Distinct().Count() == 1)
            return false;
        
        var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        
        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;
        
        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        
        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        
        var digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();
        
        return cpf.EndsWith(digito);
    }
    
    private static bool ValidarCnpj(string cnpj)
    {
        if (cnpj.Length != 14)
            return false;
        
        // Verifica se todos os dígitos são iguais
        if (cnpj.Distinct().Count() == 1)
            return false;
        
        var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        
        var tempCnpj = cnpj.Substring(0, 12);
        var soma = 0;
        
        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
        
        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        
        var digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;
        
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
        
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();
        
        return cnpj.EndsWith(digito);
    }
}