using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	/// <summary>
	/// Description of ScreenBr.
	/// </summary>
	class ScreenBr : AnimatedSprite
	{
		public Vector2 ScreenRes { get; set; } 
		
		public ScreenBr(Vector2 ScreenRes, int Frames, int Start_Alpha, GraphicsDevice gr, float Layer = BasicSprite.DefaultLayer) : base(Vector2.Zero, new Texture2D(gr, Frames, 1), 1, 60, Layer)
		{
			var DataArr = new Color[Frames];
			for(var i = 0; i < Frames; i++)
			{
				var chng = i * ((float)Start_Alpha / (Frames - 1));
				DataArr[i] = new Color(0,0,0,Start_Alpha - (int)chng);
			}
			DataArr[Frames - 1] = new Color(0,0,0,0);
			Texture.SetData(DataArr);
			AddAnimation("ScreenBr+", 0, Frames - 1, false);
			this.ScreenRes = ScreenRes;
		}
		
		public void ScreenBrUp()
		{
			PlayAnimation("ScreenBr+");
		}
		
		public override void Draw(SpriteBatch Target)
		{
			if (Visible)
				Target.Draw(Texture, null, new Rectangle(Position.ToPoint(), ScreenRes.ToPoint()), new Rectangle(Convert.ToInt32(CurrentFrame*_FrameSize.X), 0, 1, 1), null, 0f, Vector2.Zero, null, SpriteEffects.None, Layer);
		}
	}
}
