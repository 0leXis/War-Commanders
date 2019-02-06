/*
 * Created by SharpDevelop.
 * User: User
 * Date: 21.01.2019
 * Time: 16:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	/// <summary>
	/// Description of ContentLoader.
	/// </summary>
	public static class ContentLoader
	{
		private static ContentManager content;
		
		public static void Init(ContentManager Content)
		{
			content = Content;
		}
		
		public static Texture2D LoadTexture(string Path)
		{
			return content.Load<Texture2D>(Path);
		}
		
		public static SpriteFont LoadFont(string Path)
		{
			return content.Load<SpriteFont>(Path);
		}

        public static string LoadScript(string Path)
        {
            using (var Fil = new StreamReader(Path, Encoding.Default))
                return Fil.ReadToEnd();
        }

	}
}
