// Decompiled with JetBrains decompiler
// Type: RummageAttributes.RummageKeepReflectionSafeAttribute
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;

namespace RummageAttributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
  [RummageKeepUsersReflectionSafe]
  public sealed class RummageKeepReflectionSafeAttribute : Attribute
  {
  }
}
