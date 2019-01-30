
function Init(Sender)
	local Data = {}
	Data.Bullet = BasicSprite(Sender.Position, ContentLoader.LoadTexture('Textures\\Bullet'), 0.1)
	return Data
end

function Update(Sender, Data, IsAttack)
	if(Data.Bullet.Position.X > 0) then
		Data.Bullet.Position = Vector2(Data.Bullet.Position.X - 10, Data.Bullet.Position.Y)
	else
		IsAttack = false
	end
	return Data, IsAttack
end

function Draw(Sender, Target, Data)
	Data.Bullet:Draw(Target);
	return Data
end