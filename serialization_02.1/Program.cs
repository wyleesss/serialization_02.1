using System.Text;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System;

using Payment;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        SoapFormatter formatter = new SoapFormatter();

        InvoiceForPayment payment = new InvoiceForPayment(1000m, 14, 100m, 3);
        InvoiceForPayment trueResult = null;
        InvoiceForPayment falseResult = null;

        Console.WriteLine("ОРИГІНАЛ:\n");
        Console.WriteLine(payment);

        using (Stream fStream = File.Create("true.soap"))
            formatter.Serialize(fStream, payment);

        InvoiceForPayment.IsSerializable = false;

        using (Stream fStream = File.Create("false.soap"))
            formatter.Serialize(fStream, payment);

        Console.WriteLine("\n\nРЕЗУЛЬТАТ СЕРІАЛІЗАІЇ ЗІ ЗНАЧЕННЯМ ПОЛЯ [IsSerializable] \"true\":\n");

        using (Stream fStream = File.OpenRead("true.soap"))
            trueResult = formatter.Deserialize(fStream) as InvoiceForPayment;
        
        Console.WriteLine(trueResult);
        Console.WriteLine("\n\nРЕЗУЛЬТАТ СЕРІАЛІЗАІЇ ЗІ ЗНАЧЕННЯМ ПОЛЯ [IsSerializable] \"false\":");

        using (Stream fStream = File.OpenRead("false.soap"))
            falseResult = formatter.Deserialize(fStream) as InvoiceForPayment;
        
        Console.WriteLine(falseResult);
        Console.ReadKey();
    }
}