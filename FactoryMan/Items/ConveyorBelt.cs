using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace FactoryMan.Items
{
  public class ConveyorBelt : Object
  {
    public ConveyorBelt()
    {
      name = "Conveyor Belt";
      Description = "Moves, extracts, and inserts items.";
      Texture = "ConveyorBelt";
      Category = 0;
      IsPassable = true;
      IsPlaceable = true;
      bigCraftable = true;
      boundingBox = new Rectangle(0, 0, 0, 0);
      DrawPosition = new Vector2(0, 0);
      SourceRect = new Rectangle(0, 0, 16, 32);
      MaxStackSize = 999;
      fragility = fragility_Removable;
      Rotation = 0;
      Items = new List<Debris>();

      type = "Crafting";
    }

    public override string Name
    {
      get { return name; }
      set { name = value; }
    }
    
    public string Description { get; set; }
    public string Texture { get; set; }
    public string CategoryName { get; set; }
    public Color CategoryColour { get; set; }
    public bool IsPassable { get; set; }
    public bool IsPlaceable { get; set; }
    public bool HasBeenRegistered { get; set; }
    public int RegisteredId { get; set; }
    public Rectangle SourceRect { get; set; }
    public int Rotation { get; set; }
    public List<Debris> Items { get; set; }

    public int MaxStackSize { get; set; }

    public bool WallMounted { get; set; }
    public Vector2 DrawPosition { get; set; }

    public bool FlaggedForPickup { get; set; }

    [XmlIgnore]
    public Vector2 CurrentMouse { get; protected set; }

    [XmlIgnore]
    public Vector2 PlacedAt { get; protected set; }

    public override int Stack
    {
      get { return stack; }
      set { stack = value; }
    }

    public override string getDescription()
    {
      return Description;
    }

    public virtual Rectangle getSourceRect()
    {
      Rectangle ret;
      if (SourceRect == null)
        ret = Game1.currentLocation.getSourceRectForObject(ParentSheetIndex);
      else
        ret = SourceRect;
      ret.X = 16 * Rotation;
      return ret;
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
    {
      if (Texture != null)
      {
        if (this.bigCraftable)
        {
          Vector2 vector2 = this.getScale() * (float)Game1.pixelZoom;
          Vector2 globalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
          Rectangle destinationRectangle = new Rectangle((int)((double)globalPosition.X - (double)vector2.X / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int)((double)globalPosition.Y - (double)vector2.Y / 2.0) + (this.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), (int)((double)Game1.tileSize + (double)vector2.X), (int)((double)(Game1.tileSize * 2) + (double)vector2.Y / 2.0));
          spriteBatch.Draw(FactoryMan.ItemTextures[Texture], destinationRectangle, getSourceRect(), Color.White * alpha, 0.0f, Vector2.Zero, SpriteEffects.None, 1E-09f);
        }
        else
          spriteBatch.Draw(FactoryMan.ItemTextures[Texture], Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize + Game1.tileSize / 2 + (shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0), y * Game1.tileSize + Game1.tileSize / 2 + (shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0))), this.getSourceRect(), Color.White * alpha, 0f, new Vector2(8f, 8f), scale.Y > 1f ? getScale().Y : Game1.pixelZoom, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-09f);
      }
      foreach(var debris in this.Items)
        debris.item.drawInMenu(spriteBatch, Game1.GlobalToLocal(Game1.viewport, debris.Chunks[0].position), (float)(0.800000011920929 + (double)debris.itemQuality * 0.100000001490116), 1f, (float)(((double)(debris.chunkFinalYLevel + Game1.tileSize) + (double)debris.Chunks[0].position.X / 10000.0) / 10000.0), false);
      if (!this.readyForHarvest)
        return;
      float num = (float)(4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 8), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((double)((y + 1) * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + (double)this.tileLocation.X / 10000.0 + (this.parentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
      spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16)), Color.White * 0.75f, 0.0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((double)((y + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + (double)this.tileLocation.X / 10000.0 + (this.parentSheetIndex == 105 ? 0.00150000001303852 : 0.0)));
    }

    public new void drawAsProp(SpriteBatch b)
    {
    }

    public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
    {
      Log.Debug("THIS DRAW FUNCTION IS NOT IMPLEMENTED I WANT TO KNOW WHERE IT IS CALLED");
      //try
      //{
      //    if (Texture != null)
      //    {
      //        int targSize = Game1.tileSize;
      //        int midX = (xNonTile) + 32;
      //        int midY = (yNonTile) + 32;

      //        int targX = midX - targSize / 2;
      //        int targY = midY - targSize / 2;

      //        Rectangle targ = new Rectangle(targX, targY, targSize, targSize);
      //        spriteBatch.Draw(Texture, targ, null, new Color(255, 255, 255, 255f * alpha), 0, Vector2.Zero, SpriteEffects.None, layerDepth);
      //        //spriteBatch.Draw(Program.DebugPixel, targ, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
      //        /*
      //        spriteBatch.DrawString(Game1.dialogueFont, "TARG: " + targ, new Vector2(128, 0), Color.Red);
      //        spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 0.5f, targY), Color.Orange);
      //        spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX, targY), Color.Red);
      //        spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 1.5f, targY), Color.Yellow);
      //        spriteBatch.DrawString(Game1.dialogueFont, ".", new Vector2(targX * 2f, targY), Color.Green);
      //        */
      //    }
      //}
      //catch (Exception ex)
      //{
      //    Log.AsyncR(ex.ToString());
      //    Console.ReadKey();
      //}
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      if (isRecipe)
      {
        transparency = 0.5f;
        scaleSize *= 0.75f;
      }

      if (Texture != null)
      {
        var targSize = (int)(64 * scaleSize * 0.9f);
        var midX = (int)(location.X + 32);
        var midY = (int)(location.Y + 32);

        var targSizeX = (int)(64 * scaleSize * 0.9f);
        var targSizeY = (int)(64 * scaleSize * 0.9f);

        if(this.bigCraftable)
          targSizeX = (int)(targSizeX * 0.5);

        var targX = midX - targSizeX / 2;
        var targY = midY - targSizeY / 2;

        spriteBatch.Draw(FactoryMan.ItemTextures[Texture], new Rectangle(targX, targY, targSizeX, targSizeY), getSourceRect(), new Color(255, 255, 255, transparency), 0, Vector2.Zero, SpriteEffects.None, layerDepth);
      }
      if (drawStackNumber)
      {
        var _scale = 0.5f + scaleSize;
        Game1.drawWithBorder(stack.ToString(), Color.Black, Color.White, location + new Vector2(Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(stack.ToString())).X * _scale, Game1.tileSize - (float)((double)Game1.tinyFont.MeasureString(string.Concat(stack.ToString())).Y * 3.0f / 4.0f) * _scale), 0.0f, _scale, 1f, true);
      }
    }

    public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
    {
      if (Texture != null)
      {
        var midX = (int)(objectPosition.X + 32);
        var midY = (int)(objectPosition.Y + 32);

        var targSizeX = 64;
        var targSizeY = 64;

        if (this.bigCraftable)
          targSizeY *= 2;

        var targX = midX - targSizeX / 2;
        var targY = midY - targSizeY / 2;

        spriteBatch.Draw(FactoryMan.ItemTextures[Texture], new Rectangle(targX, targY, targSizeX, targSizeY), this.getSourceRect(), Color.White, 0, Vector2.Zero, SpriteEffects.None, (f.getStandingY() + 2) / 10000f);
      }
    }

    public override Color getCategoryColor()
    {
      if (CategoryColour == null)
        return base.getCategoryColor();
      return CategoryColour;
    }

    public override string getCategoryName()
    {
      if (string.IsNullOrEmpty(CategoryName))
        return base.getCategoryName();
      return CategoryName;
    }

    public override bool isPassable()
    {
      return IsPassable;
    }

    public override bool isPlaceable()
    {
      return IsPlaceable;
    }

    public override int maximumStackSize()
    {
      return MaxStackSize;
    }

    public ConveyorBelt Clone()
    {
      var toRet = new ConveyorBelt();

      toRet.Name = Name;
      toRet.CategoryName = CategoryName;
      toRet.Description = Description;
      toRet.Texture = Texture;
      toRet.IsPassable = IsPassable;
      toRet.IsPlaceable = IsPlaceable;
      toRet.quality = quality;
      toRet.scale = scale;
      toRet.isSpawnedObject = isSpawnedObject;
      toRet.isRecipe = isRecipe;
      toRet.questItem = questItem;
      toRet.stack = 1;
      toRet.HasBeenRegistered = HasBeenRegistered;
      toRet.RegisteredId = RegisteredId;
      toRet.type = type;
      toRet.bigCraftable = bigCraftable;
      toRet.boundingBox = boundingBox;
      toRet.SourceRect = SourceRect;
      toRet.DrawPosition = DrawPosition;
      toRet.fragility = fragility;
      toRet.Rotation = Rotation;

      return toRet;
    }

    public override Item getOne()
    {
      return Clone();
    }

    public override void actionWhenBeingHeld(Farmer who)
    {
      var x = Game1.getOldMouseX() + Game1.viewport.X;
      var y = Game1.getOldMouseY() + Game1.viewport.Y;

      x = x / Game1.tileSize;
      y = y / Game1.tileSize;

      CurrentMouse = new Vector2(x, y);
      //Program.LogDebug(canBePlacedHere(Game1.currentLocation, CurrentMouse));
      base.actionWhenBeingHeld(who);
    }

    public override bool canBePlacedHere(GameLocation l, Vector2 tile)
    {
      //Program.LogDebug(CurrentMouse.ToString().Replace("{", "").Replace("}", ""));
      return base.canBePlacedHere(l, tile);
      //if (!l.objects.ContainsKey(tile))
      //  return true;
      //return false;
    }

    public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
    {
      if (Game1.didPlayerJustRightClick())
      {
        Game1.playSound("dwoop");
        this.Rotation++;
        if (this.Rotation > 3)
          this.Rotation = 0;
        return false;
      }

      x = x / Game1.tileSize;
      y = y / Game1.tileSize;

      //Program.LogDebug(x + " - " + y);
      //Console.ReadKey();

      var key = new Vector2(x, y);

      if (!canBePlacedHere(location, key))
        return false;

      var s = Clone();

      s.health = 10;
      s.owner = who == null ? Game1.player.uniqueMultiplayerID : who.uniqueMultiplayerID;
      s.PlacedAt = key;
      s.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, boundingBox.Width, boundingBox.Height);

      location.objects.Add(key, s);
      Log.Async($"{GetHashCode()} - {s.GetHashCode()}");

      return true;
    }

    public override void actionOnPlayerEntry()
    {
      //base.actionOnPlayerEntry();
    }

    public override void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
    {
      if (canBePlacedHere(location, CurrentMouse))
      {
        var targSize = Game1.tileSize;

        var x = Game1.getOldMouseX() + Game1.viewport.X;
        var y = Game1.getOldMouseY() + Game1.viewport.Y;
        spriteBatch.Draw(Game1.mouseCursors, new Vector2(x / Game1.tileSize * Game1.tileSize - Game1.viewport.X, y / Game1.tileSize * Game1.tileSize - Game1.viewport.Y), new Rectangle(Utility.playerCanPlaceItemHere(location, this, x, y, Game1.player) ? 194 : 210, 388, 16, 16), Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.01f);
        this.draw(spriteBatch, x / Game1.tileSize, y / Game1.tileSize, 0.5f);
      }
    }

    public override void updateWhenCurrentLocation(GameTime time)
    {
      didToolActionThisTick = false;
      updateConveyorBelt(Game1.currentLocation);
      base.updateWhenCurrentLocation(time);
    }

    [XmlIgnore]
    int ticksUntilExtract = 60;
    public void updateConveyorBelt(GameLocation location)
    {
      if (Game1.paused)
        return;
      bool isCurrentLocation = (Game1.currentLocation == location);
      int destX = Rotation == 0 ? 1 : Rotation == 2 ? -1 : 0;
      int destY = Rotation == 1 ? 1 : Rotation == 3 ? -1 : 0;
      Rectangle thisTileRect = new Rectangle((int)PlacedAt.X * Game1.tileSize, (int)PlacedAt.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      Point posFront = new Point(thisTileRect.X + (destX * Game1.tileSize), thisTileRect.Y + (destY * Game1.tileSize));
      Point posBack = new Point(thisTileRect.X - (destX * Game1.tileSize), thisTileRect.Y - (destY * Game1.tileSize));
      Object destinationObject = location.getObjectAt(posFront.X, posFront.Y);
      Object connectedObject = location.getObjectAt(posBack.X, posBack.Y);
      List<Debris> removedDebris = new List<Debris>();
      foreach (var i in location.debris)
        if (i.item != null && i.Chunks[0].yVelocity == 0 && thisTileRect.Contains((int)i.Chunks[0].position.X + Game1.tileSize, (int)i.Chunks[0].position.Y + Game1.tileSize))
        {
          this.Items.Add(createConveyorItem(i.item, i.Chunks[0].position));
          removedDebris.Add(i);
        }
      foreach (var i in removedDebris)
        location.debris.Remove(i);
      removedDebris.Clear();
      foreach (var debris in this.Items)
      {
        Chunk chunk = debris.Chunks[0];
        if (destinationObject != null)
        {
          if (destinationObject is Chest)
          {
            Chest chest = destinationObject as Chest;
            Item oldItem = debris.item;
            Item newItem = chest.addItem(debris.item);
            if (newItem == null)
            {
              if (isCurrentLocation)
                Game1.playSound("Ship");
              removedDebris.Add(debris);
            }
            else if (newItem.Stack != oldItem.Stack)
            {
              if (isCurrentLocation)
                Game1.playSound("Ship");
              debris.item = newItem;
            }
          }
          else if ((debris.item is Object) && destinationObject.performObjectDropInAction(debris.item as Object, true, Game1.player))
          {
            destinationObject.performObjectDropInAction(debris.item as Object, false, Game1.player);
            if (isCurrentLocation)
              Game1.playSound("Ship");
            debris.item.Stack--;
            if (debris.item.Stack <= 0)
              removedDebris.Add(debris);
          }
          else if (destinationObject is ConveyorBelt && !(destinationObject as ConveyorBelt).hasConveyorItemAt((int)chunk.position.X + (destX * Game1.tileSize), (int)chunk.position.Y + (destY * Game1.tileSize)))//!(destinationObject as ConveyorBelt).doesDebrisIntersect(new Rectangle((int)chunk.position.X + destX, (int)chunk.position.Y + destY, Game1.tileSize, Game1.tileSize)))
          {
            if (!hasConveyorItemAt((int)chunk.position.X + (destX * Game1.tileSize), (int)chunk.position.Y + (destY * Game1.tileSize), debris))
            {
              chunk.position.X += destX;
              chunk.position.Y += destY;
            }
            if ((destX != 0 && chunk.position.X == posFront.X) || (destY != 0 && chunk.position.Y == posFront.Y))
            {
              (destinationObject as ConveyorBelt).Items.Add(debris);
              removedDebris.Add(debris);
            }
          }
        }
        else if ((debris.item is Object) && (location is Farm) && !(posFront.X / Game1.tileSize < 71 || (posFront.X / Game1.tileSize > 72 || posFront.Y / Game1.tileSize < 13) || (posFront.Y / Game1.tileSize > 14 || !(debris.item as Object).canBeShipped())))
        {
          (location as Farm).shippingBin.Add(debris.item);
          (location as Farm).lastItemShipped = debris.item;
          (location as Farm).showShipment(debris.item as Object, false);
          removedDebris.Add(debris);
        }
      }
      foreach (var i in removedDebris)
        this.Items.Remove(i);
      Vector2 placementPos = new Vector2(thisTileRect.X, thisTileRect.Y);
      if (!doesDebrisIntersect(new Rectangle((int)placementPos.X, (int)placementPos.Y, Game1.tileSize, Game1.tileSize)) && connectedObject != null)
      {
        if (connectedObject.heldObject != null && connectedObject.readyForHarvest)
        {
          Debris item = createConveyorItem(connectedObject.heldObject, placementPos);
          this.Items.Add(item);
          connectedObject.heldObject = null;
          connectedObject.readyForHarvest = false;
        }
        else if (connectedObject is Chest)
        {
          Chest chest = connectedObject as Chest;
          for (var i = 0; i < chest.items.Count; i++)
            if (chest.items[i] != null && chest.items[i].Stack > 0)
            {
              Debris item = createConveyorItem(chest.items[i].getOne(), placementPos);
              this.Items.Add(item);
              chest.items[i].Stack--;
              if (chest.items[i].Stack <= 0)
                chest.items[i] = null;
              break;
            }
        }
      }
    }

    public bool doesDebrisIntersect(Rectangle rect, Debris self=null)
    {
      foreach (var debris in this.Items)
      {
        if (debris.item == null || debris.item.Stack <= 0)
          continue;
        if (self != null && debris == self)
          continue;
        var chunk = debris.Chunks[0];
        Rectangle thisDebrisRect = new Rectangle((int)chunk.position.X, (int)chunk.position.Y, Game1.tileSize, Game1.tileSize);
        if (thisDebrisRect.Intersects(rect))
          return true;
      }
      return false;
    }

    public bool hasConveyorItemAt(int x, int y, Debris exclude=null)
    {
      foreach (var debris in this.Items)
      {
        if (debris.item == null || debris.item.Stack <= 0)
          continue;
        if (exclude != null && debris == exclude)
          continue;
        var chunk = debris.Chunks[0];
        //Rectangle thisDebrisRect = new Rectangle((int)chunk.position.X, (int)chunk.position.Y, Game1.tileSize, Game1.tileSize);
        if (chunk.position.X == x && chunk.position.Y == y)
          return true;
      }
      return false;
    }

    public Debris createConveyorItem(Item item, Vector2 pos)
    {
      Debris ret = new Debris(-2, 1, pos, pos, 0f);
      ret.item = item;
      ret.Chunks[0].position += new Vector2(Game1.tileSize / 2, Game1.tileSize / 2);
      return ret;
    }

    public override bool minutesElapsed(int minutes, GameLocation environment)
    {
      return base.minutesElapsed(minutes, environment);
    }

    bool didToolActionThisTick = false;
    public override bool performToolAction(Tool t)
    {
      bool ret = base.performToolAction(t);
      if (ret && !(t is Pickaxe))
        return false;
      if (ret)
        didToolActionThisTick = true;
      return ret;
    }

    public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
    {
      if (didToolActionThisTick)
      {
        environment.debris.Add(new Debris(getOne(), Game1.player.GetToolLocation(true)));
        this.fragility = 2;
      }
      foreach(var debris in this.Items)
        environment.debris.Add(new Debris(debris.item, debris.Chunks[0].position));
      base.performRemoveAction(tileLocation, environment);
    }
  }
}
