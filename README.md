# UtilityPDF
This software was created, totally free, to facilitate and collect in one place, some operations that are normally done on PDF files.

Currently the possible operations are:

	* compression PDF file by being able to adjust the compression factor;
	* extraction text from PDF, via OCR and being able to select different languages for recognition of the text to be extracted;
	* merge PDF files by carefully following the sequence of adding files, creating a new PDF file;
	* convert pdf to DOCX and/or RTF file
	  Convert pdf file in editable DOCX and/or RTF. >>> Not necessary office installed <<< to convert.
	  If PDF contain only image, output DOCX and/or RTF file will have only image. Use extraction text (with OCR) to convert text
	  inside an image.


Used library:

	* FreeSpire.Doc		v. 12.2.0	(https://www.e-iceblue.com/Introduce/free-doc-component.html)
						Free Spire.Doc for .NET is a Community Edition of the Spire.Doc
						for .NET, which is a totally free word API for commercial and personal use.
	* Freeware.Pdf2Png	v. 1.0.1 	MIT License
	* Freeware.Pdf2Docx	v. 1.1.0 	MIT License
	* Ghostscript.NET	v. 1.2.3.1	AGPL (GNU Affero General Public License)
	* PDFsharp		v. 6.1.1	MIT License	
	* Tesseract		v. 5.2.0 	Apache License
	
	All library dependencies, mentioned above, are MIT LICENSED

 
For TESSERACT traineddata (LSTM	only - best) put in tessdata directory trained languages.
Downolad languages to https://github.com/tesseract-ocr/tessdata_best
	
This software is released under the MIT license

[2024] [Giovanni Limongiello aka Firefox_1998]
