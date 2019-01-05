/*
 * Created by SharpDevelop.
 * User: 215-02
 * Date: 04.01.2019
 * Time: 15:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	/// <summary>
	/// Description of IDrawable
	/// </summary>
	public interface IDrawable
	{
		void Draw(SpriteBatch Target);
	}
}
