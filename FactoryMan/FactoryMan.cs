using FactoryMan.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Cat = StardewValley.Characters.Cat;

namespace FactoryMan
{
  public class FactoryMan : Mod
  {
    public static Dictionary<string, Texture2D> ItemTextures = new Dictionary<string, Texture2D>();

    public override void Entry(params object[] objects)
    {
      SaveGame.serializer = new XmlSerializer(typeof(SaveGame), new Type[28]
      {
        typeof (Tool),
        typeof (GameLocation),
        typeof (Crow),
        typeof (Duggy),
        typeof (Bug),
        typeof (BigSlime),
        typeof (Fireball),
        typeof (Ghost),
        typeof (Child),
        typeof (Pet),
        typeof (Dog),
        typeof (Cat),
        typeof (Horse),
        typeof (GreenSlime),
        typeof (LavaCrab),
        typeof (RockCrab),
        typeof (ShadowGuy),
        typeof (SkeletonMage),
        typeof (SquidKid),
        typeof (Grub),
        typeof (Fly),
        typeof (DustSpirit),
        typeof (Quest),
        typeof (MetalHead),
        typeof (ShadowGirl),
        typeof (Monster),
        typeof (TerrainFeature),
        typeof (ConveyorBelt)
      });
      SaveGame.farmerSerializer = new XmlSerializer(typeof(Farmer), new Type[2]
      {
        typeof(Tool),
        typeof(ConveyorBelt)
      });
      SaveGame.locationSerializer = new XmlSerializer(typeof(GameLocation), new Type[27]
      {
        typeof (Tool),
        typeof (Crow),
        typeof (Duggy),
        typeof (Fireball),
        typeof (Ghost),
        typeof (GreenSlime),
        typeof (LavaCrab),
        typeof (RockCrab),
        typeof (ShadowGuy),
        typeof (SkeletonWarrior),
        typeof (Child),
        typeof (Pet),
        typeof (Dog),
        typeof (Cat),
        typeof (Horse),
        typeof (SquidKid),
        typeof (Grub),
        typeof (Fly),
        typeof (DustSpirit),
        typeof (Bug),
        typeof (BigSlime),
        typeof (BreakableContainer),
        typeof (MetalHead),
        typeof (ShadowGirl),
        typeof (Monster),
        typeof (TerrainFeature),
        typeof (ConveyorBelt)
      });
      ControlEvents.KeyPressed += ControlEvents_KeyPressed;
      GameEvents.LoadContent += GameEvents_LoadContent;
      GameEvents.UpdateTick += GameEvents_UpdateTick;
    }

    private void GameEvents_UpdateTick(object sender, EventArgs e)
    {
      foreach (var i in Game1.locations)
        if (Game1.currentLocation != i)
          foreach (var o in i.Objects)
            if (o.Value is ConveyorBelt)
              (o.Value as ConveyorBelt).updateConveyorBelt(i);
    }

    private void GameEvents_LoadContent(object sender, EventArgs e)
    {
      ItemTextures.Add("WarioBox", Game1.content.Load<Texture2D>("FactoryManItems\\WarioBox"));
      ItemTextures.Add("ConveyorBelt", Game1.content.Load<Texture2D>("FactoryManItems\\ConveyorBelt"));
    }

    private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
    {
      if(e.KeyPressed == Keys.F5)
      {
        /*FactoryManItem item = new FactoryManItem();
        item.Texture = "WarioBox";
        item.Name = "Wario Box";
        item.Description = "Wario box thing";
        item.IsPlaceable = true;
        item.bigCraftable = true;
        item.ParentSheetIndex = 2200;
        item.boundingBox = new Rectangle(0, 0, 16 * 4, 16 * 4);
        item.SourceRect = new Rectangle(0, 0, 16, 32);
        Game1.player.addItemToInventory(item);*/
        
        Game1.player.addItemToInventory(new ConveyorBelt());
      }
    }
  }
}
