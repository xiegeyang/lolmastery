(function () {
    let lol = {
        data: []
    };

    function loadSummonerStats() {
        let summoners = [];

        summoners.push({
            SummonerName: $('#summoner-one').find('input').val(),
            Region: $('#summoner-one').find('span.select-region').text().toLowerCase()
        });

        if ($('#summoner-two:visible').length) {
            summoners.push({
                SummonerName: $('#summoner-two').find('input').val(),
                Region: $('#summoner-two').find('span.select-region').text().toLowerCase()
            });
        }

        let param = {
            summonerSearchParam: summoners
        };

        $.ajax({
            type: "POST",
            async: false,
            url: "/Services/MasteryService.asmx/MasteryInfoBySummonerName",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        })
        .done(function (response) {
            if (response.d.ActionState) {
                lol.data = JSON.parse(response.d.ActionResult);

                displaySummonerStats(lol);

                $('html, body').animate({
                    scrollTop: $("#summoner-stats").offset().top
                }, 2000);
            } else {
                alert(response.d.Message);
            }
        })
        .fail(function (error) {
            alert(error);
        });
    }

    function displaySummonerStats(summonerData) {
        $('#summoner-stats').empty().append(
            `
            <div class="container-fluid">
            <div class="page-header text-color">
                <h1>Summoner Mastery Stats</h1>
            </div>

            ${getSummonerStatsHeader(summonerData)}

            ${getSummonerChampions(summonerData)}
            </div>
            `
        );

        getSummonerChart(summonerData);

    }


    function getSummonerStatsHeader(summonerData) {
        let statsHeader;

        if (summonerData.data.length === 1) {
            statsHeader =
                `
                <div class="jumbotron inner-shadow blue-color">
                <div class="row">
                    <div class="col-md-6 col-md-offset-3 col-xs-5">
                    <div class="row">
                        <div class="col-md-4 col-xs-12">
                        <img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/profileicon/${summonerData.data[0].Summoner.profileIconId}.png" alt="" class="img-rounded img-responsive center-block">
                        </div>
                        <div class="col-md-4 col-xs-12 text-center text-color">
                        <h2>${summonerData.data[0].Summoner.name}</h2>
                        </div>
                        <div class="col-md-4 col-xs-12">
                        <img src="./Images/${summonerData.data[0].Summoner.tier.toLowerCase()}.png" class ="img-responsive center-block tier-icon">
                        </div>
                    </div>
                    </div>
                </div>
                </div>
                `;
        } else {
            statsHeader =
                `
                <div class="jumbotron inner-shadow bi-color">
                <div class="row">
                    <div class="col-md-5 col-xs-5">
                    <div class="row">
                        <div class="col-md-4 col-xs-12">
                        <img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/profileicon/${summonerData.data[0].Summoner.profileIconId}.png" alt="" class="img-rounded img-responsive center-block">
                        </div>
                        <div class="col-md-4 col-xs-12 text-center text-color">
                        <h2>${summonerData.data[0].Summoner.name}</h2>
                        </div>
                        <div class="col-md-4 col-xs-12">
                        <img src="./Images/${summonerData.data[0].Summoner.tier.toLowerCase()}.png" class="img-responsive center-block tier-icon">
                        </div>
                    </div>
                    </div>
                    <div class="col-md-2 col-xs-2">
                        <span></span>
                    </div>
                    <div class="col-md-5 col-xs-5">
                    <div class="row">
                        <div class="col-md-4 col-xs-12 col-md-push-8">
                        <img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/profileicon/${summonerData.data[1].Summoner.profileIconId}.png" alt="" class="img-rounded img-responsive  center-block">
                        </div>
                        <div class="col-md-4 col-xs-12 text-center text-color">
                        <h2>${summonerData.data[1].Summoner.name}</h2>
                        </div>
                        <div class="col-md-4 col-xs-12 col-md-pull-8">
                        <img src="./Images/${summonerData.data[1].Summoner.tier.toLowerCase()}.png" class ="img-responsive center-block tier-icon">
                        </div>
                    </div>
                    </div>
                </div>
                </div>
                `;
        }

        return statsHeader;
    }

    function getSummonerChampions(summonerData) {
        let summonerChampions;

        if (summonerData.data.length === 1) {
            summonerChampions =
                `
                <div class="container">
                <div class="row">
                    <div class="col-md-6" style="margin-bottom: 80px;">

                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[0], true)}
                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[1], true)}
                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[2], true)}

                    <!-- Button trigger modal -->
                    <button type="button" class="btn btn-lg all-champs-btn blue-color-btn" data-toggle="modal" data-target="#myModal" data-summoner-index="0">
                        All Summoner Champions
                    </button>

                    </div>

                    <div class="col-md-6">

                    <canvas id="summonerChart" width="400" height="400"></canvas>

                    </div>
                </div>
                </div>
                `;
        } else {
            summonerChampions =
                `
                <div class="row">
                <div class="col-md-4" style="margin-bottom: 80px;">

                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[0])}
                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[1])}
                    ${getSummonerLeftTopChampion(summonerData.data[0].Champion[2])}

                    <!-- Button trigger modal -->
                    <button type="button" class="btn btn-lg all-champs-btn blue-color-btn" data-toggle="modal" data-target="#myModal" data-summoner-index="0">
                    All Summoner Champions
                    </button>

                </div>

                <div class="col-md-4">

                    <canvas id="summonerChart" width="400" height="400"></canvas>

                </div>

                <div class="col-md-4" style="margin-bottom: 80px;">

                    ${getSummonerRightTopChampion(summonerData.data[1].Champion[0])}
                    ${getSummonerRightTopChampion(summonerData.data[1].Champion[1])}
                    ${getSummonerRightTopChampion(summonerData.data[1].Champion[2])}

                    <!-- Button trigger modal -->
                    <button type="button" class="btn btn-lg all-champs-btn red-color-btn" data-toggle="modal" data-target="#myModal" data-summoner-index="1">
                    All Summoner Champions
                    </button>

                </div>
                </div>
                ` ;
        }

        return summonerChampions;
    }

    function getSummonerLeftTopChampion(champion) {
        return (
          `
          <div class="media">
            <div class="media-left media-middle">
              <img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/${champion.key}.png" alt="" class="media-object img-circle">
            </div>
            <div class="media-body text-center text-color">
              <h4 class="media-heading">${champion.name}</h4>
              <p>
                Champion level: ${champion.championLevel} <br />
                Highest grade: ${champion.highestGrade ? champion.highestGrade : ''} <br />
                Champion points: ${champion.championPoints}
              </p>
              <div class="progress">
                <div class="progress-bar blue-color" role="progressbar" aria-valuenow="${champion.championPointsSinceLastLevel}" aria-valuemin="0" aria-valuemax="${champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel}" style="width: ${Math.round((champion.championPointsSinceLastLevel * 100) / (champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel))}%;">
                  ${Math.round((champion.championPointsSinceLastLevel * 100) / (champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel))}%
                </div>
              </div>
            </div>
          </div>
          `
        );
    }

    function getSummonerRightTopChampion(champion) {
        return (
            `
                <div class="media">
                <div class="media-body text-center text-color">
                    <h4 class="media-heading">${champion.name}</h4>
                    <p>
                    Champion level: ${champion.championLevel} <br />
                    Highest grade: ${champion.highestGrade ? champion.highestGrade : ''} <br />
                    Champion points: ${champion.championPoints}
                    </p>
                    <div class="progress">
                    <div class="progress-bar red-color" role="progressbar" aria-valuenow="${champion.championPointsSinceLastLevel}" aria-valuemin="0" aria-valuemax="${champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel}" style="width: ${Math.round((champion.championPointsSinceLastLevel * 100) / (champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel))}%;">
                        ${Math.round((champion.championPointsSinceLastLevel * 100) / (champion.championPointsSinceLastLevel + champion.championPointsUntilNextLevel))}%
                    </div>
                    </div>
                </div>
                <div class="media-left media-middle">
                    <img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/${champion.key}.png" alt="" class="media-object img-circle">
                </div>
                </div>
            `
        );
    }

    function displayAllSummonerChampions(summonerIndex) {
        $('#all-summoner-champions').empty();

        let allSummonerChamps = ``;
        console.log(lol.data[summonerIndex].Champion);
        for (let champIndex = 0, totalSummonerChamps = lol.data[summonerIndex].Champion.length; champIndex < totalSummonerChamps; champIndex++) {
            allSummonerChamps = allSummonerChamps + getAllSummonerChampionByIndex(summonerIndex, champIndex);
        }

        $('#all-summoner-champions').append(allSummonerChamps);
    }

    function getAllSummonerChampionByIndex(summonerIndex, champIndex) {
        return (
            `
              <tr>
                <td><img src="http://ddragon.leagueoflegends.com/cdn/6.9.1/img/champion/${lol.data[summonerIndex].Champion[champIndex].key}.png" alt="" class="img-rounded img-responsive all-champs"></td>
                <td>${lol.data[summonerIndex].Champion[champIndex].name}</td>
                <td>${lol.data[summonerIndex].Champion[champIndex].championLevel}</td>
                <td>${lol.data[summonerIndex].Champion[champIndex].highestGrade ? lol.data[summonerIndex].Champion[champIndex].highestGrade : ''}</td>
                <td>${lol.data[summonerIndex].Champion[champIndex].championPoints}</td>
                <td><span class="glyphicon glyphicon-${lol.data[summonerIndex].Champion[champIndex].chestGranted ? 'ok' : 'remove'}"></span></td>
              </tr>
            `
        );
    }

    function getSummonerChart(summonerData) {
        let chartElement = $("#summonerChart");

        const blueRGBColor = "41,64,124";
        const redRGBColor = "102,0,0";
        // Chart Limits
        const limits = [
                       700, // total score 
                       140, // total champions
                       140, // total champions
                       140, // total champions
                       200000 // huge score
        ];

        let chartData = {
            labels: ["Score", "Champions Used", "Champions Level 5", "S Ranks", "Best Champion Points"],
            datasets: []
        };

        // Calculate statistiscs using limits to get the stat percetage and use the result as dataset for the chart
        for (let summonerIndex = 0, totalSummoners = summonerData.data.length; summonerIndex < totalSummoners; summonerIndex++) {
            let data = [];
            for (let statIndex = 0, totalStats = limits.length; statIndex < totalStats; statIndex++) {
                data.push(
                  summonerData.data[summonerIndex].SummonerStatistics[statIndex] >= limits[statIndex]
                  ? 100 : (summonerData.data[summonerIndex].SummonerStatistics[statIndex] * 100) / limits[statIndex]
                );
            }

            chartData.datasets.push({
                label: summonerData.data[summonerIndex].Summoner.name,
                backgroundColor: `rgba(${summonerIndex ? redRGBColor : blueRGBColor},0.2)`,
                borderColor: `rgba(${summonerIndex ? redRGBColor : blueRGBColor},1)`,
                pointBackgroundColor: `rgba(${summonerIndex ? redRGBColor : blueRGBColor},1)`,
                pointBorderColor: "#fff",
                pointHoverBackgroundColor: "#fff",
                pointHoverBorderColor: `rgba(${summonerIndex ? redRGBColor : blueRGBColor},1)`,
                data: data,
                realData: summonerData.data[summonerIndex].SummonerStatistics
            });
        }

        new Chart(chartElement, {
            type: 'radar',
            data: chartData,
            options: {
                legend: {
                    display: false,
                    onClick: null
                },
                scale: {
                    reverse: false,
                    ticks: {
                        maxTicksLimit: 5,
                        min: 0,
                        max: 100,
                        beginAtZero: true,
                        display: false,
                        color: "#D08504",
                    },
                    pointLabels: {
                        fontColor: "#D08504",
                    },
                    angleLines: {
                        display: true,
                        color: "#D08504",
                        lineWidth: 3
                    },
                    gridLines: {
                        color: "#D08504",
                    }
                },
                tooltips: {
                    enabled: true,
                    callbacks: {
                        label: function (tooltipItem, data) {
                            return data.datasets[tooltipItem.datasetIndex].label + ": " + data.datasets[tooltipItem.datasetIndex].realData[tooltipItem.index];
                        }
                    }
                }
            }
        });
    }

    $(function () {
        $('.search, #compare-btn').click(function () {
            let isValid = true;

            if ($('#summoner-one').find('input').val() === '' || $('#summoner-one').find('span.select-region').text() === 'Select Region') {
                isValid = false;
            }

            if ($('#summoner-two:visible').find('input').val() === '' || $('#summoner-two:visible').find('span.select-region').text() === 'Select Region') {
                alert('Please select a region and type a summoner name');
                return false;

                isValid = false;
            }

            if (isValid) {
                loadSummonerStats();
            } else {
                alert('Please select a region and type a summoner name');
            }
        });

        $('#summoner-two').hide();

        $('#compare-link').click(function () {
            $('#compare-legend').hide();
            $('.search').hide();

            $("#summoner-two").animate({
                top: '60%'
            }, 0, function () {
                $('#summoner-two').show(300);
                $('#compare').show(300, "linear");
            });
        });

        $('.region').click(function (e) {
            e.preventDefault();

            $(this).parentsUntil('.input-group').find('.select-region').text($(this).data('region'));
            $(this).parentsUntil('.input-group').parent().find('input').focus();
        });

        $('#compare-cancel-btn').click(function (e) {
            $('#summoner-two, #compare').fadeOut("fast", function () {
                $("#summoner-one").animate({
                    top: '40%'
                }, 300);

                $('#compare-legend').fadeIn();
                $('.search').show();
            });
        });

        // MODAL
        $('#myModal').on('show.bs.modal', function (event) {
            let button = $(event.relatedTarget) // Button that triggered the modal
            let modal = $(this)
            modal.find('#filter').val('');
            $('.searchable tr').show();

            displayAllSummonerChampions(button.data('summoner-index'));
        });

        $('#filter').keyup(function () {
            let rex = new RegExp($(this).val(), 'i');
            $('.searchable tr').hide();
            $('.searchable tr').filter(function () {
                return rex.test($(this).text());
            }).show();
        });
    });

}());
