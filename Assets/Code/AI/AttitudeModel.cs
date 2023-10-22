using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AttitudeModel
    {
        public Type Type { get; }
        public AttitudeType AttitudeType { get; }
        public Func<Component, float> Method { get; }

        public AttitudeModel(Type type, AttitudeType attitudeType, Func<Component, float> method)
        {
            this.Type = type;
            this.AttitudeType = attitudeType;
            this.Method = method;
        }

        public override bool Equals(object obj)
        {
            return obj is AttitudeModel other &&
                   EqualityComparer<Type>.Default.Equals(Type, other.Type) &&
                   AttitudeType == other.AttitudeType &&
                   EqualityComparer<Func<Component, float>>.Default.Equals(Method, other.Method);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, AttitudeType, Method);
        }

        public void Deconstruct(out Type type, out AttitudeType attitudeType, out Func<Component, float> method)
        {
            type = this.Type;
            attitudeType = this.AttitudeType;
            method = this.Method;
        }

        public static implicit operator (Type type, AttitudeType attitudeType, Func<Component, float> method)(AttitudeModel value)
        {
            return (value.Type, value.AttitudeType, value.Method);
        }

        public static implicit operator AttitudeModel((Type type, AttitudeType attitudeType, Func<Component, float> method) value)
        {
            return new AttitudeModel(value.type, value.attitudeType, value.method);
        }
    }
}