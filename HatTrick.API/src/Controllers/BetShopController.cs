﻿using HatTrick.API.Models;
using HatTrick.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace HatTrick.API.Controllers
{
    /// <summary>Provides endpoints for placing bets.</summary>
    [Route("API/[controller]")]
    public class BetShopController : InternalBaseController
    {
        protected readonly BetShop _betShop;

        public BetShopController(
            BetShop betShop,
            IMemoryCache cache,
            ILogger<BetShopController> logger,
            bool disposeMembers = false
        ) :
            base(cache, logger, disposeMembers)
        {
            _betShop = betShop ??
                throw new ArgumentNullException(nameof(betShop));
        }

        /// <summary>Places a new bet (ticket).</summary>
        /// <param name="ticketRequest">The new ticket information. It should contain the user's id, the list of selected outcomes' ids and the amount to bet.</param>
        /// <param name="placedAt">The date-time at which to place the bet. If omitted, current time is used.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The response.</returns>
        /// <response code="200">The bet was placed successfully.</response>
        /// <response code="400">Request failed.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] TicketRequestModel ticketRequest,
            [FromQuery] DateTime? placedAt = null,
            CancellationToken cancellationToken = default
        ) =>
            await DoActionAsync(
                () => _betShop.PlaceBetAsync(
                    placedAt.GetValueOrDefault(
                        GetDefaultTime(HttpContext)
                    ),
                    ticketRequest.UserId,
                    ticketRequest.SelectionIds.ToImmutableArray(),
                    ticketRequest.Amount,
                    cancellationToken
                )
            )
                .ConfigureAwait(false);

        protected override void Dispose(
            bool disposing
        )
        {
            if (disposing && !Disposed)
            {
                if (_disposeMembers)
                {
                    _betShop.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected override async ValueTask DisposeAsync(
            bool disposing
        )
        {
            if (disposing && !Disposed)
            {
                if (_disposeMembers)
                {
                    await _betShop.DisposeAsync()
                        .ConfigureAwait(false);
                }
            }

            await base.DisposeAsync(disposing)
                .ConfigureAwait(false);
        }
    }
}
