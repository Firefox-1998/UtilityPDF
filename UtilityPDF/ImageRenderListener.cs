using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using System;
using System.Collections.Generic;
using System.IO;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace UtilityPDF
{
    internal class ImageRenderListener : IEventListener
    {
        private readonly Body body;
        private readonly MainDocumentPart mainPart;
        private int imageCounter = 1;

        public ImageRenderListener(Body body, MainDocumentPart mainPart)
        {
            this.body = body;
            this.mainPart = mainPart;
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_IMAGE)
            {
                ImageRenderInfo renderInfo = (ImageRenderInfo)data;
                PdfImageXObject image = renderInfo.GetImage();
                if (image != null)
                {
                    AddImageToDocument(image);
                }
            }
        }

        private void AddImageToDocument(PdfImageXObject image)
        {
            byte[] imageBytes = image.GetImageBytes(true);
            string imagePartId = $"image{imageCounter++}";
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png, imagePartId);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                imagePart.FeedData(ms);
            }

            string relationshipId = mainPart.GetIdOfPart(imagePart);
            AddImageToBody(relationshipId);
        }

        private void AddImageToBody(string relationshipId)
        {
            string imageName = "Picture_" + Guid.NewGuid().ToString();
            string editId = Guid.NewGuid().ToString("N").Substring(0, 8);

            var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent() { Cx = 990000L, Cy = 792000L },
                        new DW.EffectExtent()
                        {
                            LeftEdge = 0L,
                            TopEdge = 0L,
                            RightEdge = 0L,
                            BottomEdge = 0L
                        },
                        new DW.DocProperties()
                        {
                            Id = (UInt32Value)1U,
                            Name = imageName
                        },
                        new DW.NonVisualGraphicFrameDrawingProperties(
                            new A.GraphicFrameLocks() { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties()
                                        {
                                            Id = (UInt32Value)0U,
                                            Name = imageName
                                        },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension()
                                                {
                                                    Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                })
                                        )
                                        {
                                            Embed = relationshipId,
                                            CompressionState = A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(
                                            new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0L, Y = 0L },
                                            new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                        new A.PresetGeometry(
                                            new A.AdjustValueList()
                                        )
                                        { Preset = A.ShapeTypeValues.Rectangle })))
                            { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                            )
                        )
                    {
                        EditId = editId
                    });

            body.AppendChild(new Paragraph(new Run(element)));
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new HashSet<EventType> { EventType.RENDER_IMAGE };
        }
    }
}
