# UtilityPDF
This software was created to facilitate, and collect in one place, some operations that are normally done on PDF files.
Currently the possible operations are:

	* compression PDF file by being able to adjust the compression factor;
	* extraction text from PDF, via OCR and being able to select different languages for recognition of the text to be extracted;
	* merge PDF files by carefully following the sequence of adding files, creating a new PDF file.
	* convert (basic) pdf to DOCX (only text and image. No OCR, No formatting text. Only convert pdf page - text, img, and other, in editable docx)

Used library:
    - DocumentFormat.OpenXml	v. 3.1.0	MIT License	
	- Freeware.Pdf2Png			v. 1.0.1 	MIT License
	- Ghostscript.NET			v. 1.2.3.1	AGPL (GNU Affero General Public License)
    - iText7					v. 8.0.5	AGPL (GNU Affero General Public License)
    - PDFsharp					v. 6.1.1	MIT License	
    - Tesseract					v. 5.2.0 	Apache License
	
	All library dependencies mentioned above are MIT LICENSE
	
This software is released under the MIT license