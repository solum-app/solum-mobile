using System.Threading.Tasks;
using System.IO;

namespace Solum.Interfaces
{
	public interface IPdfViewer
	{
		void PreviewPdf(MemoryStream stream);
	}
	public interface ISaveWindowsPhone
	{
		Task PreviewPdf(MemoryStream stream);
	}
}


