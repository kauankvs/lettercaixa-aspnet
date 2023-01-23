﻿using LettercaixaAPI.Models;
using LettercaixaAPI.Services.Implementations;
using LettercaixaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LettercaixaAPI.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoriteMoviesController : ControllerBase
    {
        private readonly IFavoriteService _service;
        public FavoriteMoviesController(IFavoriteService service) => _service = service;

        [HttpPut]
        [Route("add")]
        [Authorize]
        public async Task<ActionResult<FavoriteMovie>> AddMovieToFavoritesAsync(int movieId)
            => await _service.AddMovieToFavoritesAsync(User.FindFirstValue(ClaimTypes.Email), movieId);

        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public async Task<ActionResult> RemoveMovieFromFavoritesAsync(int movieId)
            => await _service.RemoveMovieFromFavoritesAsync(User.FindFirstValue(ClaimTypes.Email), movieId);
    }
}