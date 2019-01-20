
function StartAttack(Sender)
	Log.SendMessage('It works')
	math.randomseed(os.time())
end

function Update(Sender)
	Sender.Position = Vector2(math.random(1,3000), math.random(1,3000));
end

function Draw(Sender, Target)
	
end