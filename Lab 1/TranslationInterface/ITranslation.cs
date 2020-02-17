using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationInterface
{
    public interface ITranslation 
    {
        string Translate(string source);
        string GetStudentId();
        string GetName();
    }
}
