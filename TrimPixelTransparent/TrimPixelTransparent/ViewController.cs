using System;
using CoreGraphics;
using UIKit;

namespace TrimPixelTransparent
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			//Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ivOrginal.Image = UIImage.FromBundle("duc");
			btnTrim.TouchUpInside += BtnTrim_TouchUpOutside;

			// Perform any additional setup after loading the view, typically from a nib.
		}

		void BtnTrim_TouchUpOutside(object sender, EventArgs e)
		{
			var image = UIImage.FromBundle("duc");
			var image1 = ImageByTrimmingTransparentPixelsRequiringFullOpacity(true, image);
			ivAfterTrim.Image = image1;

			SaveImage(image1);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public  UIEdgeInsets TransparencyInsetsRequiringFullOpacity(bool fullyOpaque, UIImage image)
		{
			//Draw our image on that context
			int width = (int)image.CGImage.Width;
			int height = (int)image.CGImage.Height;
			int bytesPerRow = width;


			// Allocate array to hold alpha channel
			byte[] bitmapData = new byte[width * height];

			// Create alpha-only bitmap context
			CGContext contextRef = new CGBitmapContext(bitmapData, width, height, 8, bytesPerRow, null, CGImageAlphaInfo.Only);
			CGImage cgImage = image.CGImage;
			CGRect rect = new CGRect(0, 0, width, height);
			contextRef.DrawImage(rect, cgImage);


			// Sum all non-transparent pixels in every row and every colum
			ushort[] rowSum = new ushort[height];
			ushort[] colSum = new ushort[width];

			// Enumrate throung all pixels
			for (int row = 0; row < height; row++)
			{
				for (int col = 0; col < width; col++)
				{
					if (fullyOpaque)
					{
						//Foud non-transparent pixel
						if (bitmapData[row * bytesPerRow + col] == 255)
						{
							rowSum[row]++;
							colSum[col]++;
						}
					}
					else
					{
						// Found non-transparent pixel
						if ((row * bytesPerRow + col) <= bitmapData.Length)
						{
							rowSum[row]++;
							colSum[col]++;
						}
					}
				}
			}

			// Initalize crop insets and enumerate cols/rows arrays until we find non-empty colums or row
			UIEdgeInsets crop = new UIEdgeInsets();

			//Top
			for (int i = 0; i < height; i++)
			{
				if (rowSum[i] > 0)
				{
					crop.Top = i;
					break;
				}
			}

			//Bottom
			for (int i = height - 1; i >= 0; i--)
			{
				if (rowSum[i] > 0)
				{
					crop.Bottom = Math.Max(0, height - i - 1);
					break;
				}
			}

			//Left
			for (int i = 0; i < width; i++)
			{
				if (colSum[i] > 0)
				{
					crop.Left = i;
					break;
				}
			}

			//Right
			for (int i = width - 1; i >= 0; i--)
			{
				if (colSum[i] > 0)
				{
					crop.Right = Math.Max(0, width - i - 1);
					break;
				}
			}

			return crop;
		}

		public  UIImage ImageByTrimmingTransparentPixelsRequiringFullOpacity(bool fullyOpaque, UIImage image)
		{
			if (image.Size.Height < 2 || image.Size.Width < 2)
			{
				return image;
			}
			CGRect rect = new CGRect(0, 0, image.CGImage.Width, image.CGImage.Height);

			UIEdgeInsets crop = TransparencyInsetsRequiringFullOpacity(fullyOpaque, image);

			UIImage img = image;

			if (crop.Top == 0 && crop.Bottom == 0 && crop.Left == 0 && crop.Right == 0)
			{
				//No cropping needed;
			}
			else
			{
				
				// Calculate new crop bounds
				rect.X += crop.Left;
				rect.Y += crop.Top;
				rect.Width -= crop.Left + crop.Right;
				rect.Height -= crop.Top + crop.Bottom;

				// Crop it
				CGImage newImage = image.CGImage.WithImageInRect(rect);

				//Convert back to UIImage
				img = UIImage.FromImage(newImage);

			}
			return img;
		}

		public void SaveImage(UIImage imageSave)
		{
			imageSave.SaveToPhotosAlbum((image, error) =>
			{
				var o = image as UIImage;
				Console.WriteLine("error:" + error);
			});
		}
	}
}
