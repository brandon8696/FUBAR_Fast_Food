This is the Unity project read me for FUBAR FAST FOOD. Copying the GDD from google right now. 

Game Concept Brainstorm: 
Working Title: FUBAR Fast-Food

Summary: Absolute rip-off of a low fidelity Puppet Combo style game. 3rd person action shooter with Resident Evil style inventory and puzzles. Game takes place at a haunted plantation owned by a deceased disgruntled fast-food mogul who collected franchise merchandise and displays on their decrepit Louisiana property. 
Carl Junior’s old scary fry guy was the major inspiration. 

Story Summary: General Cindy ran several successful fast food franchises until he was wrongfully pushed out by the corporate board owners over his dedication to honest ethics in the early 80s. Angry and driven mad, he began obsessively collecting franchise merchandise on his property and dabbling in Cajun voodoo black-magic. Ancient spirits of the swamp stole his soul as it became one with the Plantation (final boss), and the franchise mascots overran the property by the 1990s. 

You are a cable television team running a live 1997 Halloween all-night movie marathon. The broadcast network keeps you suffering for the ratings as you fight for your life against the evil Fast-Food forces of General Cindy. 

🎮 Game Design Document: [FUBAR FAST FOOD]
1. Game Overview
Title: FUBAR Fast Food


Genre: Survival Action Horror, PSX Low Poly Action Horror


Platform(s): Steam


Target Audience: Teens (Probably M-Rated Anyway)


Game Summary: You are a cable television team running a live 1997 Halloween all-night movie marathon. The broadcast network keeps you suffering for the ratings as you fight for your life against the evil Fast-Food forces of General Cindy on his haunted Louisiana plantation. (General Cindy’s Ragin’ Cajun Plantation)


Unique Selling Points: Low fidelity PSX style action horror game incorporating elements of southern gothic styling and the quirky unsettling nature of fast-food mascots.


2. Gameplay
Core Mechanics: 3rd person survival shooter where the player must kill deranged possessed fast food mascots while unlocking doors with keys, solving puzzles, and unravelling the mystery of the plantation.


Player Actions: Would prefer modern-ish 3rd person player controller that can aim and shoot while walking or moving the camera. Player can interact with items they have in inventory and use them for health or puzzles. Player can move puzzle item. 
Upon playing several retro 3rd person games on Playstation 1, including Tomorrow Never Dies, Die Hard Trilogy, Dino Crisis 2, and a few others, I have concluded that I like the handling model of Tomorrow Never Dies the best, and think it would be fantastic with modern twin still controls. 
Tomorrow Never Dies Has: Forward, Back, Rotate, Dive, Crouch, and Aim. Forward movement combined with left or right d-pad commands result in player lean. I don’t think we need crouching, but would be cool. I also would like ladder climbing, but may avoid it. 
I do want reload animations
I do want a weapon wheel w/ time freeze
Interact button for doors, scripted items, scripted comments, etc…
Unity 3rd Person Controller: https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131 NOT USING
Roll animations + free anims: 
https://assetstore.unity.com/packages/3d/animations/free-32-rpg-animations-215058 
https://www.youtube.com/watch?v=FbM4CkqtOuA
https://www.youtube.com/watch?v=kwpaJ-YzY5E
Counter Strike Source/Half Life style colliders for bullet/weapon hit detection…
https://discussions.unity.com/t/how-to-handle-hitbox-skeleton-for-characters/127260



Combat / Stealth Systems: 3rd person shooting, with verticality and possible ladders. Melee combat too. No stealth, just line of sight for monsters with music cue and maybe eye color. 
Firearms
AMT Automag 30cal (Low ROF Med Damage)
M2 Carbine Full Auto (Med ROF Med Damage)
Mosin Nagant (High Damage Bolt Action)
Uzi 9MM (High ROF Low Damage)
M1897 Combat Pump (High Damage Med ROF Spread-Shot)
Steyr AUG (Med Damage High ROF)
Melee Weapons 
Machete
Chainsaw
Would be cool if Melee had a down and up slash if tapped quickly. Can Increase damage output during fights.
Explosives
Thumper (Apocalypse Now ) (Low ROF High Damage with AOE)
Dynamite (Low Range Low ROF High Damage with AOE)
PLAYER CONTROLLER SYSTEM
Forward
Backward
Strafe Left (AIMING ONLY)
Strafe Right (AIMING ONLY)
Dive Left (replaced with single forward dive)
Dive Right (replaced with single forward dive)
Shooting/Attack
Zoom camera + pan right (smooth it)
Lower max run speed by ¾
Shooting animation
Reload animation
Interact animation
Death animation
Hit/Damage Animation



Inventory / Resource Management: Inventory will consist of health, puzzle items, and maybe power ups. Stretch goal would be weapon add-ons and collectables


Puzzle Design: Key retrieval, sundial puzzle, fuse retrieval, and explosions?


Enemies: *idea, load enemies with random textures or color variants to give visual variety. 
Fry Guy - rush enemies with tackle/ as well as bite. 
Monster Shake - can bite enemies upclose, or waddle and shoot at range
Bovine Burger Brute - heavy character that can rush, punch, and shoot at range
Hognado - flying slow enemy with high health, range attack, and close gnaw attack. (Think Tazmanian Devil Tornado with a Frycook Pig on Top)
Texas Weiner - medium character, medium speed, medium ranged attack, close punch
Mr.Softy - Soft Serve Ice Cream Cone that has similar stats to Texas Weiner, but shoots liquid napalm from a soft serve dispenser (water stops it)
Gators - bite attack and tail whip
Crawfish - like rats
Civil War/Slave Ghoul - Basic shuffling crusty zombie with swipe attack, brought back to life by ethereal hamburger
Dynamite Foul - Deep Fried Whole Chicken with a stick of dynamite sticking out of the head. Runs at player and explodes. 


ENEMY BEHAVIOR: Follow Doom style behavior. Enemies can get knocked back by weapons, but so can players. 
160 degree cone line of sight with diminishing change of spotting depending on distances
Monsters generally try to close in on their target via the shortest possible route. If the direct route to the target is blocked by a wall or a solid thing that the monster walks into, it randomly picks another direction to move in for a short while (to find another route to its target) until it reverts to heading straight toward its target again.Monsters generally try to close in on their target via the shortest possible route. If the direct route to the target is blocked by a wall or a solid thing that the monster walks into, it randomly picks another direction to move in for a short while (to find another route to its target) until it reverts to heading straight toward its target again.
If a monster has no target, it is in the dormant state and will not move OR follow a patrol route.
The further a monster is from its target, the more it will use its ranged attack (if it has one), and the closer, the less. The logic behind this is, that when the monster is in close proximity of its target, it will try to get to its target faster to be able to hit it with its melee attack.
	
Bosses: 
General Cindy: The head honcho himself. General CIndy has been horrifically mutated by occult black-magic into a towering monstrous southern gentleman. Free his soul from evils he had brought unto this world. 

Super Sized Shake: A classic over-sized giant bullet sponge boss. The shake operates much like the smaller shake, except can do a ground shaking slam AOE attack. Also shoots homing missiles. 
Gator Bait: Wendy’s Knock-Off riding a possessed swamp gator
Slay Palace: The animated screeching horror comprised of an old Play Palace, Game Station, and various other fast food merch. It screeched an ethereal groan and shoots exploding children’s corpses out of the slide. Also ball-pit AOE bomb attack. 


3. Story & Setting
Narrative Overview: Halloween 1997 and a cable television station’s fun spooky special turns into a nightmare, as they get trapped on the Louisiana Plantation of General Cindy. General Cindy was a fast food mogul, who was driven insane by being forced out of his company. He began hoarding fast food memorabilia and dabbling into voodoo occult magic as a way to win back his legacy. General Cindy disappeared several years prior and his plantation had fallen abandoned. Whether it be the trespassing or gates of the underworld opening up on Halloween night, the television crew is forced to fight for survival against waves of possessed demonic fast-food mascots and mutated swamp critters. Each level has you pushing closer to the answers of what happened to General Cindy and his spiral into black magic. 
Story ends with a climactic battle against the possessed soul of Cindy occupying a giant fast food mascot. 


Main Characters: TV Personality Dusty Derringer, Co-Host Kimmy Song, and Cameraman Sammy Kielbasa. 
Dusty Derringer: 43 year old Vietnam Vet with a loudmouth sarcastic attitude, but an honorable moral compass. Accidentally landed a gig in cable television after an HBO special featuring his thrill seeking antics and bombastic personality. Enjoys hunting, the outdoors, bad movies, fast cars, and a good cigar. 
Kimmy Song: 27 year old daughter of immigrants from Vietnam, she was raised by American television and movies in the 70s. She dreamed to get into entertainment, and after earning her degree in media from the University of Texas she found herself wrapped up in the booming cable television industry. This has been her first year hosting a show on a network. 
Sammy Kielbasa: Sammy was a combat videographer during the early days of the Vietnam conflict, with 10 years seniority on Dusty. While he prefers his action days behind him, he has enjoyed a steady career in Atlanta’s television industry. 

Setting & Time Period: Halloween 1997 General Cindy’s Louisiana plantation. Starting at dusk and going until morning. 


Tone / Themes: VHS analog horror with themes of fast-food nostalgia and 90s cheese.


Lore Expanded: General Cindy’s was and is a world-wide fast-food franchise created by the mogul, James Cindy. Mr.Cindy was no General, but rather a southern businessman with a long-running family history dating back to pre-colonial America. His family has had the Cindy Plantation for over 250 years, with a legacy of slavery, gun running, prohibition smuggling, and various business investments. General Cindy’s started life as a small family diner ran out of New Orleans, and quickly grew in popularity as America’s highway system expanded. By the 1970s the franchise had grown across the United States with a simplified menu of fried chicken, shrimp, burgers, hotdogs, and french fries. By the early 80s it had gone international and James had legally changed his name to “General Cindy”
The success was short lived, as James Cindy ran into legal issues by the mid 80s as he tried to remove his family from long-standing illegal ventures, but multiple scandals forced him to sell off his majority shares. By the early 90s Cindy was forced off the board, his beloved wife had passed away, and he fell into a downward spiral, collecting General Cindy’s merchandise while vowing to get his fortune back. By 1996 he had disappeared and the property fell abandoned while in legal limbo. 
Dusty Derringer, Kimmy Song, and Sammy Kielbasa drive from their cable studio in Atlanta Georgia over 6 hours to Cindy’s Plantation in Eastern Louisiana. 
The crew sets up for an all-night Halloween horror movie marathon on location for the Vodun TV channel (The Name of a West African Traditional Religion, a nod to voodoo and veve.).
4. Level Design

Number of Levels / Areas: Prologue: Crew sets up inside General Cindy's Plantation at Dusk and Kimmy disappears. 
Look For Kimmy on the Plantation Entrance Grounds.
Large open front yard with a long driveway that runs to the front gate. Whole plantation home is on a peninsula. Swamp/Water on both sides. Lines of bald cypress trees w/ spanish moss on either side of path with front water fountain. Front door is locked, gate is locked, cemetery gate is covered in vines and garage is closed. Fireflies everywhere. Old fast food memorabilia strewn around. Spooky mascot is now missing.
Find Machete leaning on tree. Optional AMT if trigger event (stretch goal)
Approaching cemetery gate prompts Kimmy’s scream beyond gate
Use Machete to chop vines covering cemetery gate. Machete might prompt first melee fight with a Fry Guy.
Locked into the family Cemetery 
The family cemetery is an above ground burial site of every Cindy family member for generations. It’s an elegant southern gothic design akin to New Orleans or any other swampland cemetery in the south. It dates back to the late 1600s during the French occupation. 
Level opens with old General Cindy’s sign falling in front of gate. Only way out finding a way to the back yard and greenhouse. Cindy sign has piece of puzzle that falls off (Cindy’s face)
Gate to backyard can only be opened after performing a grave puzzle. 
Key to mausoleum. Key on weeping angel. Mausoleum has tomb inside. Place the face of Cindy in the casket. Casket closes, and the front gate releases. 	
Upon return to the opened gate, The Demonic Shake Boss hops over the gate to wreak havoc. 
Escape to the Greenhouse and back yard. 
The player escapes to the plantation’s back yard. A once luxurious yard with gardens, fountains, hedges, trees, and several greenhouses. It also has a gazebo, recreational area, and pool. It is now a maze of hazards, monsters, and the occult
The house is inaccessible, but you see an air boat across the swamp. You want to escape and get help. You need to lower a bridge to get there, and to lower the bridge you need a crank. Crank is located on the Slay Palace. The Slay Palace can be accessed via the Sun Dial that must be turned to reveal a key. 
Sun Dial is IN Greenhouse and has to be set to 8pm
Sun Dial Key Opens Playground
Entering the playground triggers the Slay Palace boss cutscene and the user must defeat the boss to get the crank.
Swamp/Bueyu level
The Bueyu level is a series of small interconnected islands filled with gators, crawfish, and the most decrepit of fast food monsters. The islands were where slaves and servants lived in overgrown shacks. Now they are haunted with the echoes of voodoo black magic. 
Upon entering the level, the air boat is instantly destroyed by Gator Bait, who stalks you relentlessly around the level
Water is a soft death zone that gets spammed with Gators and Crawfish until the player is dead. There are also points where if the player goes below a certain point in the water, they get insta-damage. 
The interconnected Islands all have blessed items to destroy the voodoo cursed barriers blocking the area of progression. 
Level wraps back around the peninsula, and returns back to the other side of the plantation home, with General Cindy’s personal dock connecting with the swamp shanty village. You don’t find a boat, but you are confronted by Gator Bait. Defeat them and progress to the interior of the plantation. 
Old Plantation Interior 
3 stories and a basement of close-quarters combat through the Colonial styled mansion. The inside is a little slower paced, with the user truly putting the pieces together of General Cindy’s descent into occult madness. The house has turned to ethereal hamburger, with hornet-nest like structures of dripping meat. The lines of reality and underworld are starting to blur. 
Straight up key hunt from the basement to the 3rd floor/attic. 
3rd floor triggers an event where GC busts through the floor and flings the player back into the front yard. 
Final General Cindy Fight at Entrance
Re-use of area 1, but now set at day-break. The Plantation home is destroyed, and the property is in ruin. Cindy climbs through the wreckage of the building and towards your location. 
Would be cool as a multi-level boss. Fighting the giant statue and then the head only ala Twisted Metal 2 Dark Tooth. 
Final Cut Scene: Ends with police busting through the front gate. Kim is unconscious in the destroyed head of CIndy, and you and the cameraman are seen looking over her. Roll credits. 
Progression Structure:


Environmental Themes: Southern Gothic and Rural Swampy with the contrast of bright fast food junk. (Think an American Pickers property in Louisiana with monsters) The entire plantation consists of the main mansion and entrance road, a pre colonial era cemetery, greenhouse, outdoor Gazebo and event space, as well as the ruins of old shanties from tenant farmers and slaves. The entire property is neurotically packed with tens of thousands of pieces of fast food memorabilia and restaurant pieces. 
5. Visual & Audio Style
Art Style (Reference Examples): Analog PSX/Early PS2 low poly horror. 
Note expand this with real examples and set limitations. 
Ideally I want to limit it to say Dreamcast level fidelity, with PSX style jagginess
Probably 256x256 texture sizes at max
GTA 3 had human polycounts under 1500
Dreamcast max screen resolution was 640x480
wobbly/jaggy PSX style rendering and lighting


UI/UX Notes: Television cable channel overlays. Wobbly and crunched textures. 


Sound Design: Analog and crunchy, like you are listening through an old CRT television.


Music Direction: Since I am commissioning music tracks, I can actually expand on the music I want for the game. 
This game is set in the deep south of the United States during the 90s. 
This is an 3rd person action horror game
I would like a slower spooky melodic track that plays as the player is exploring or wandering around. 
I would like a faster, more energetic track that plays when the player is in a fight or action sequence. 
Please note, as I get these levels done and the game takes shape, I will probably commission for a unique music track per level to make it feel more fresh and evoke the tone of the level. 
My main inspirations are acoustic guitar, harmonica, electric guitar, synth and ambient noise. I love the tracks of 70s and 80s low budget horror movies, especially the Italian variety, but I’m also a huge fan of 80s and 90s metal. I would love a fun mixture of that fuzed into traditional southern cajun/jazz. I think it will be creatively challenging, but wonderful if pulled off. 
Tremors 1, Beast Wars, Respect The Wind by Van Halen, Twisted Metal 1 (PS1), Cajun Hell by Exodus, Twisted Metal Small Brawl, House By The Cemetery, Zombi 2
 Twisted Metal 1 has a fantastic use of harmonica and metal riffs. Small Brawl less so, but still great. 
Respect The Wind is a beautiful slow melodic metal instrumental 
Tremors 1’s soundtrack and Cajun Hell both demonstrate fantastic themed genre music. 
House By The Cemetery and Zombi 2 are two perfect low budget Italian horror flicks with great budget soundtracks. 
Tremors 1 OST Sample 
Cajun Hell - Exodus
Twisted Metal 1 OST Sample
Respect The Wind - Eddie Van Halen
Zombi 2 Guitar Cover 
House By The Cemetery OST
Beast Wars Theme
Demoni Theme


6. Technical Info
Engine: Unity


Development Tools / Plugins: ChatGPT, Automated Marketing Bots, Automated Texture Gen Tools. Automated Music Gen Tools. Blender for models and animation


Target Performance (FPS, resolution, etc): 4:3 120fps.


7. Production
Estimated Budget: 500 for initial music. 100 for Steam, and 250 for good practical art. Shoe string, but paying where it counts.  


Team Roles (even if it’s just you): 
Programing: me
Art: me
Design: me
Animation: me
Cutscenes: Me and maybe Ryan?
Music: (Andrew Hulshult, McGlocky, etc?) (probably a pipe dream) 
Cover Art: ? 
Voice overs: Me, Alexa, Ryan? (Who has a high quality mic?)
Voice Editing: Who can crunch-ify the voice recordings and sound fx?
Sound FX?: Public domain, old 90s sfx cds? Me and Ryan?
Publication: Me
Marketing: Ryan and Me? Pitch to New Bloods? 


Estimated Timeline: 8 months. 


Dependencies (Assets, Commissions, Licenses):
Unity Engine
Commission Music (Explore AI)
Commission Cover Art (Explore AI)
Need open source music/sfx
Legal contracts
Small marketing budget
Unity premium fee, Steam Fee, Itch.io, and maybe GOG?
https://textures.neocities.org/


8. Extra Juice / Goals
Voice Lines for…
Weapon pickups
Multi-Kills
Level Openings
Key/Door Unlocks
Boss Fights
Texture variants per enemy
Normal/Swampy/Fleshy
Occult Symbols/Spanish Moss
Graphics settings
Extra crunchy, VHS, scanlines vs no scanlines
3D model wobble and texture quality
Dusk style pixelization slider
Cut Scenes
Opening Cutscene
RE style zoom-in shots
Level Opening Cutscenes
Boss Intro/Death Cutscenes
Ending Cutscene
SOUND
Swamp ambience (Frogs/Toads Crickets etc)
Flies, mosquitoes, fireflies, and dragonflies particle fx
Water splashing, grass rustling, etc
Gibbing/Gore
Blow enemies the fuck apart
Blood splatters/Blood pools
Interactable Text Observations (RE Style for Fluff/Story)
9. Assets
	Textures
Grass texture
Mud texture
Swamp water texture
Stone texture
Wood plank texture
	Models 
bald cypress trees
Spanish Moss
Cindy Mansion Exterior 
Swamp cattails
Swamp lilies
Lilly Pads
Grass
Thorn Bushes
Water ripple and water splash effect/texture
Fire fly and regular fly particle effect
Fire effect
Explosion effect
Gun barrel blast effect
Ground beef piles
Blood splatter texture
Blood/Viscera drip effect? (could be recolored into slime too) 
Blood pool texture
Bullet hole texture
Generic bullet impact particle effect (Can shoot ground, walls, objects, etc, and this is what instantiates on impact)
Spider web/cob webs 

10. Inspirations 
Murder House (Steam)
Jungle Rot (Steam)
Dusk (Steam)
BloodWash (Steam)
The Thing Remastered
– Lower List is Actual Retro 3rd Person Shooters –
Dino Crisis 2 (PS1)
Siphon Filter 2 (PS1)
Diehard Trilogy (PS1)
007 Tomorrow Never Dies (PS1)
Silent Hill
90/00s Cable Television: 




70s and 80s B-movie horror movies. Italian and Spanish zombie movies such as Zombi 2, Burial Ground, and Tombs of the Blind Dead. Also inspired by Friday the 13th, Texas Chainsaw Massacre, Motel Hell, Hard Rock Zombies, Madman, etc. I want that grit, graininess, atmosphere, and sleaze. 







11. Script/Storyboard
OPENING: 
EXT. LOUISIANA GENERAL CINDY PLANTATION – DUSK
The *SCREECH* of a rusty metal gate cries out as the VODUN TV Van slowly rolls down the once elegant front court yard of decrepit Cindy Estate. The small crew of 3 look around at their unfamiliar surroundings. Crickets and swamp critters buzz as the ambience of nightfall washes over the swampland.
DUSTY: 
Hope ya’ll slept good at the roach motel, because it’s about to be a long night
KIMMY: 
This is my big break with the network. I’ve been studying the script for the marathon, and I’m ready. 
SAMMY: 
I’ll run a final check on the equipment, but we should be good to broadcast. The Vodun station back in Atlanta should be ready to intercept on our mark. 
CUT TO: Camera pan of the team pulling up to the front of the Cindy mansion and parking with a brake squeal. Cue van’s sliding door and door opening sound effects. 
CUT TO: Close up of crew in front of van looking up at the mansion. 
DUSTY: 
Ol’ Cindy has been missing for over a year now. The local sheriff investigated the property for a welfare check, and after coming up empty, the state took over. 
KIMMY: 
I’m assuming you greased a few wheels to let us film here? 

DUSTY: 
Let’s just say nobody will be bothering us. 
SAMMY: 
Well folks, it’s almost showtime, let’s get our places. 
FADE TO BLACK: 
CUT TO: CRT static to a cheesy cable tv logo overlay with a halloween all-night marathon theme and fairuse spooky theme music. The logo fades to our host and co-host. Both standing in front of the camera holding mics with the Cindy mansion behind them and various spooky mascots in the background strewn in the front yard. 

DUSTY: 
Alright!!! Welcome to Vodun’s 3rd annual all-night Halloween movie marathon. I’m your host Dusty Derringer, and with me is our new co-host! 
KIMMY: 
Hey marathon maniacs, I’m Kimmy Song, and I can’t wait to be your host on this journey tonight! 
DUSTY: 
We’re live right now in the heart of Louisiana at the haunted Plantation of General Cindy…
KIMMY: 
Yes, the fast food mogul General Cindy has been missing for over a year, and we’ll be investigating claims that grounds are being stalked by the other worldly apparitions of the underworld. 

DUSTY:
Honestly folks, I think he was just a disgruntled old hoarder who couldn’t let go of his empire. He more than likely turned himself into deep fried gator bait out there in these swamps. 

CAMERA PANS UP TO MANSION: 
KIMMY: 
In between breaks we’ll be exploring Cindy’s mansion and estate, so don’t take too long refilling the popcorn bowl. 


















12. Release Strategy 
Itch.io free demo
GOAL: Demo will be an “endless” wave based open level. The endless template can be used to add content and replayability to the game. The endless will have waves that slowly increase enemy count and weapon drops. 1 25% health pack per wave. 
Have all weapons functional
Have 3-5 enemy types.
Have pickups
Have health system and death state
Uber simple overlord script for maintaining rounds, spawn locations, and spawn rates per round. 
TV Guide Style Main Menu w/ Settings

Steam Early Access 
Weekly updates blasted to social media 
Send to game infuencers/steamers, plus make a list of folks to send it to. 50+ people. Use folks who showed off Puppet Combo’s work, Jungle Rott, etc. Pick genre streamers 
13. Programming
This section contains programming related brainstorms, wireframes, and systems information 
Weapons/Inventory
Make Player Inventory Class
Use C# Dictionary to Define Weapons 
`Dictionary<string, WeaponData> weaponCatalog;`
Use ScriptableObjects and Prefabs to hold weapon stats/health/pickup info. No need to use an overlord. 
using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public int magSize;
    public float fireRate;
    public GameObject weaponPrefab;
}


	

	






