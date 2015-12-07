using System.Linq;
using ClashOfClans.Util.Csv;

namespace ClashOfClans.Util.Parsers
{
    public sealed class ClashCsvParser : DataRowParser
    {
        private readonly CsvTable _csvTable;
        private object _baseRow;

        public ClashCsvParser(CsvTable table)
        {
            _csvTable = table;
        }

        protected override int RowCount
            => _csvTable.Rows.Count;

        protected override T ParseRow<T>(int row)
        {
            var item = new T();
            var parsingProperties = ParseHelper.GetParsingProperties(typeof (T));
            var newBase = false;

            foreach (var parsingProperty in parsingProperties)
            {
                var value = "";
                if (parsingProperty.Attribute.ColumnNames != null)
                {
                    value = parsingProperty.Attribute.ColumnNames.Aggregate(value, (current, columnName)
                        => current + GetData(columnName, row));
                }
                else
                {
                    value = GetData(parsingProperty.ColumnName, row);
                    if (!newBase && string.IsNullOrEmpty(value))
                    {
                        var baseValue = parsingProperty.Property.GetValue(_baseRow);
                        parsingProperty.Property.SetValue(item, baseValue);
                        continue;
                    }
                }

                ParseHelper.SetConvertedValue(parsingProperty, item, value);
                newBase = true;
            }

            if (newBase)
                _baseRow = item;

            return item;
        }

        private string GetData(string columnName, int rowIndex)
        {
            var columnIndex = _csvTable.Columns.IndexOf(columnName);
            return _csvTable.Rows[rowIndex].ItemArray[columnIndex].ToString();
        }
    }
}