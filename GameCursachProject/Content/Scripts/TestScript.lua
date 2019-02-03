
function Init(Sender, AttackedUnit)
	local Data = {}

	Data.IterationNeed = math.sqrt((AttackedUnit.Position.Y - Sender.Position.Y) * (AttackedUnit.Position.Y - Sender.Position.Y) + (AttackedUnit.Position.X - Sender.Position.X) * (AttackedUnit.Position.X - Sender.Position.X)) / 32
	Data.Iteration = 0
	local k = (AttackedUnit.Position.Y - Sender.Position.Y) / (AttackedUnit.Position.X - Sender.Position.X)
	local x = math.sqrt(1000 / (1 + k * k))
	local y = x * (AttackedUnit.Position.Y - Sender.Position.Y) / (AttackedUnit.Position.X - Sender.Position.X)
	if(AttackedUnit.Position.Y > Sender.Position.Y) then
		if(k < 0) then
			Data.Plus = Vector2(-x, -y)
		else
			Data.Plus = Vector2(x, y)
		end
	else
		if(k < 0) then
			Data.Plus = Vector2(x, y)
		else
			Data.Plus = Vector2(-x, -y)
		end
	end

	k = math.atan(k)
	local kgrad = k * 180 / math.pi
	if(kgrad > -30 and kgrad < 30 ) then
		if(AttackedUnit.Position.X > Sender.Position.X) then
			Sender:SetFrame(7)
		else
			Sender:SetFrame(6)
		end
	elseif(kgrad > -60 and kgrad < -30 ) then
		if(AttackedUnit.Position.Y > Sender.Position.Y) then
			Sender:SetFrame(1)
		else
			Sender:SetFrame(4)
		end
	elseif(kgrad > 30 and kgrad < 60 ) then
		if(AttackedUnit.Position.Y > Sender.Position.Y) then
			Sender:SetFrame(5)
		else
			Sender:SetFrame(2)
		end
	elseif(kgrad > 60 or kgrad < -60 ) then
		if(AttackedUnit.Position.Y > Sender.Position.Y) then
			Sender:SetFrame(0)
		else
			Sender:SetFrame(3)
		end
	end

	Data.EndPoint = AttackedUnit.Position + AttackedUnit.FrameSize / 2
	Data.Bullet = BasicSprite(Sender.Position + Sender.FrameSize / 2, ContentLoader.LoadTexture('Textures\\Bullet'), Vector2(0,5), k, 0.4)
	Data.Explosion = AnimatedSprite(Data.EndPoint - Vector2(50), ContentLoader.LoadTexture('Textures\\BABAH'), 100, 20, 0.1)
	Data.Explosion:AddAnimation('BABAH', 0, 14, false)
	Data.Explosion.Visible = false
	Data.LoopOn = true
	return Data
end

function Update(Sender, Data, IsAttack)
	Data.Explosion:UpdateAnims()
	if(Data.LoopOn) then
		if(Data.Iteration < Data.IterationNeed) then
			Data.Bullet.Position = Data.Bullet.Position + Data.Plus
			Data.Iteration = Data.Iteration + 1
		else
			Data.Explosion:PlayAnimation('BABAH')
			Data.Explosion.Visible = true
			Data.Bullet.Visible = false
			Data.LoopOn = false
		end
	else
		if(Data.Explosion.CurrAnimName == 'NONE') then
			IsAttack = false
		end
	end
	return Data, IsAttack
end

function Draw(Sender, Target, Data)
	Data.Bullet:Draw(Target)
	Data.Explosion:Draw(Target)
	return Data
end