﻿namespace SOURCE.Models;

public class EntityOptions
{
    public bool BuildDto { get; set; } = true;
    public bool BuildRepository { get; set; } = true;
    public bool BuildService { get; set; } = true;
    public bool BuildController { get; set; } = true;
}