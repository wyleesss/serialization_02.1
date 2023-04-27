using System.Runtime.Serialization;
using System;

namespace Payment
{
    [Serializable]
    class InvoiceForPayment : ISerializable
    {
        public static bool IsSerializable { get; set; } = true;

        public char    CurrencySign      { get; set; }
        public decimal PaymentForDay     { get; set; }
        public ushort  PaymentDaysCount  { get; set; }
        public decimal DayOfDelayPenalty { get; set; }
        public ushort  DaysOfDelayCount  { get; set; }

        public decimal AmountWithoutPenalties => PaymentForDay * PaymentDaysCount;
        public decimal PenaltyAmount => DayOfDelayPenalty * DaysOfDelayCount;
        public decimal TotalAmount => AmountWithoutPenalties + PenaltyAmount;

        public InvoiceForPayment() { }
        public InvoiceForPayment(decimal paymentForDay, ushort paymentDaysCount,
                                 decimal dayOfDelayPenalty, ushort daysOfDelayCount,
                                 char currencySign = '$')
        {
            CurrencySign = currencySign;
            PaymentForDay = paymentForDay;
            PaymentDaysCount = paymentDaysCount;
            DayOfDelayPenalty = dayOfDelayPenalty;
            DaysOfDelayCount = daysOfDelayCount;
        }

        private InvoiceForPayment(SerializationInfo info, StreamingContext context)
        {
            try
            {
                CurrencySign = info.GetChar("ВАЛЮТА");
                PaymentForDay = info.GetDecimal("ОПЛАТА ЗА ДЕНЬ");
                PaymentDaysCount = info.GetUInt16("К-СТЬ ДНІВ ОПЛАТИ");
                DayOfDelayPenalty = info.GetDecimal("ШТРАФ ЗА ДЕНЬ ЗАТРИМКИ ОПЛАТИ");
                DaysOfDelayCount = info.GetUInt16("К-СТЬ ДНІВ ЗАТРИМКИ ОПЛАТИ");
            }
            catch (SerializationException)
            {
                Console.WriteLine("\n(!) серіалізація об'єкту була заблокована.\n" +
                                  "поля об'єкту отримали значення за замовчуванням\n");
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (IsSerializable)
            {
                info.AddValue("ВАЛЮТА", CurrencySign);
                info.AddValue("ОПЛАТА ЗА ДЕНЬ", PaymentForDay);
                info.AddValue("К-СТЬ ДНІВ ОПЛАТИ", PaymentDaysCount);
                info.AddValue("ШТРАФ ЗА ДЕНЬ ЗАТРИМКИ ОПЛАТИ", DayOfDelayPenalty);
                info.AddValue("К-СТЬ ДНІВ ЗАТРИМКИ ОПЛАТИ", DaysOfDelayCount);
            }
        }

        public override string ToString()
        {
            return "[РАХУНОК ДЛЯ ОПЛАТИ]\n" +
                   $"оплата за день         -> [{PaymentForDay}{CurrencySign}]\n" +
                   $"к-сть днів оплати      -> |{PaymentDaysCount}|\n" +
                   $"штраф за день затримки -> [{DayOfDelayPenalty}{CurrencySign}]\n" +
                   $"к-сть днів затримки    -> |{DaysOfDelayCount}|";
        }
    }
}