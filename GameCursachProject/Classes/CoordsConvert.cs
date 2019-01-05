/*
 * Created by SharpDevelop.
 * User: User
 * Date: 02.01.2019
 * Time: 13:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCursachProject
{
	/// <summary>
	/// Description of CoordsConvert.
	/// </summary>
	static class CoordsConvert
	{
		static public Vector2 WorldToWindowCoords(Vector2 Coords, Camera cam)
		{
			var MousePos = (Coords - cam.Position) * cam.Zoom;
        	return MousePos;
		}
		
		static public Vector2 WindowToWorldCoords(Vector2 Coords, Camera cam)
		{
			var MousePos = Coords / cam.Zoom + cam.Position;
        	return MousePos;
		}
	}
}
