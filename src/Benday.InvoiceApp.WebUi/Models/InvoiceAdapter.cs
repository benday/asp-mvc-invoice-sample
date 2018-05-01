using Benday.InvoiceApp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benday.InvoiceApp.WebUi.Models
{
    public class InvoiceAdapter
    {
        public void Adapt(Invoice fromValue, Invoice toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentNullException(nameof(fromValue), $"{nameof(fromValue)} is null.");
            }

            if (toValue == null)
            {
                throw new ArgumentNullException(nameof(toValue), $"{nameof(toValue)} is null.");
            }

            toValue.InvoiceDate = fromValue.InvoiceDate;
            toValue.InvoiceNumber = fromValue.InvoiceNumber;

            if (fromValue.InvoiceLines == null)
            {
                fromValue.InvoiceLines = new List<InvoiceLine>();
            }

            if (toValue.InvoiceLines == null)
            {
                toValue.InvoiceLines = new List<InvoiceLine>();
            }

            Adapt(fromValue.InvoiceLines, toValue.InvoiceLines);
        }

        private void Adapt(List<InvoiceLine> fromValue, List<InvoiceLine> toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentException($"{nameof(fromValue)} is null.", nameof(fromValue));
            }

            if (toValue == null)
            {
                throw new ArgumentException($"{nameof(toValue)} is null.", nameof(toValue));
            }

            InvoiceLine toLine;

            foreach (var fromLine in fromValue)
            {
                if (fromLine.Id == 0)
                {
                    toLine = new InvoiceLine();

                    toValue.Add(toLine);
                }
                else
                {
                    toLine = (from temp in toValue
                              where temp.Id == fromLine.Id
                              select temp).FirstOrDefault();

                    if (toLine == null)
                    {
                        throw new InvalidOperationException(String.Format("Invoice line collection did not contain Id '{0}'.", fromLine.Id));
                    }
                }

                Adapt(fromLine, toLine);
            }
        }

        private void Adapt(InvoiceLine fromValue, InvoiceLine toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentNullException(nameof(fromValue), $"{nameof(fromValue)} is null.");
            }

            if (toValue == null)
            {
                throw new ArgumentNullException(nameof(toValue), $"{nameof(toValue)} is null.");
            }

            toValue.ItemName = fromValue.ItemName;
            toValue.ItemDescription = fromValue.ItemDescription;
            toValue.Quantity = fromValue.Quantity;
            toValue.Value = fromValue.Value;
        }

    }
}
