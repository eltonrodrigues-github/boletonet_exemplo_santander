using BoletoNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 5; i++)
            {


                var cedente           = new Cedente();
                cedente.CPFCNPJ       = "03884532000127";
                cedente.Nome          = "IMAGEPRO SISTEMAS E PRODUTOS LTDA - EPP";
                cedente.ContaBancaria = new ContaBancaria("2259", "4", "013000672", "5");
                cedente.Convenio      = 6709028;
                cedente.Codigo        = "6709028";
                cedente.MostrarCNPJnoBoleto = true;


                var sacado             = new Sacado();
                sacado.Nome            = "FENACOR-Fed.N.C.S.P.C.PP.E.C.S.Re";
                sacado.CPFCNPJ         = "42.565.922/0001-71";
                sacado.Endereco.End    = "Rua Senador Dantas, 74 / 10º andar";
                sacado.Endereco.Bairro = "Centro";
                sacado.Endereco.Cidade = "Rio de Janeiro";
                sacado.Endereco.UF     = "RJ";
                sacado.Endereco.CEP    = "20031-180";


                var boleto              = new Boleto();
                boleto.LocalPagamento   += " Grupo Santander - GC";
                boleto.Banco            = new Banco(33);
                boleto.DataVencimento   = DateTime.Now.AddMonths(i);
                boleto.DataProcessamento = DateTime.Now;
                boleto.DataDocumento    = DateTime.Now;
                boleto.ValorBoleto      = 250;
                boleto.NossoNumero      = "000000000026";
                boleto.NumeroDocumento  = (500 + i).ToString();
                boleto.Carteira         = "101";
                boleto.Cedente          = cedente;
                boleto.Sacado           = sacado;
                boleto.Aceite           = "N";
                boleto.EspecieDocumento = new EspecieDocumento_Santander("2");//DuplicataMercantil
                boleto.Instrucoes       = new List<IInstrucao>() { new Instrucao_Santander() { Descricao = "Não receber após 5 dias de atraso" } };


                //remessa
                var remessa = new ArquivoRemessa(TipoArquivo.CNAB400);
                var boletos = new Boletos();
                boletos.Add(boleto);
                var remessaPath = string.Format(@"C:\Users\elton\Desktop\infoview\BoletoNet\boletos\santander_remessa_{0}.txt", i);
                var fileStream = new FileStream(remessaPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                remessa.GerarArquivoRemessa("6709028", new Banco(33), cedente, boletos, fileStream, 1);
                fileStream.Close();
                fileStream.Dispose();

                //boleto html
                var boleto_bancario = new BoletoBancario();
                boleto_bancario.CodigoBanco = 033;
                boleto_bancario.Boleto      = boleto;
                boleto_bancario.Boleto.Valida();
                //var boletoHtml = boleto_bancario.MontaHtmlEmbedded();
                var boletoPdf = boleto_bancario.MontaBytesPDF();

                //var boletoHtmlPath = string.Format(@"C:\Users\elton\Desktop\infoview\BoletoNet\boletos\santander_boleto_{0}.html", i);
                var boletoPdfPath = string.Format(@"C:\Users\elton\Desktop\infoview\BoletoNet\boletos\santander_boleto_{0}.pdf", i);

                //File.WriteAllText(boletoHtmlPath, boletoHtml);
                File.WriteAllBytes(boletoPdfPath, boletoPdf);
            }
        }
    }
}
