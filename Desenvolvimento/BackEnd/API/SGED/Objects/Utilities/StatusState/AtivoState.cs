﻿using Microsoft.EntityFrameworkCore;
using SGED.Context;
using SGED.Objects.Interfaces;

namespace SGED.Objects.Utilities.StatusState
{
    public class AtivoState : IStatusState
    {
        public string State { get; } = "Ativo";
        public bool CanEdit() => true;
        public bool CanRelate() => true;
        public bool CanToRemove() => true;
    }
}
