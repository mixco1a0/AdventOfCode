
using System.Collections.Generic;

namespace AoC
{
    public enum TestPart
    {
        One,
        Two
    }

    class TestDatum
    {
        public TestPart TestPart { get; set; }
        public string RawInput { private get; set; }
        private IEnumerable<string> m_input = null;
        public IEnumerable<string> Input
        {
            get
            {
                if (m_input == null)
                {
                    m_input = Util.ConvertInputToList(RawInput);
                }
                return m_input;
            }
        }
        public string Output { get; set; }
    }
}