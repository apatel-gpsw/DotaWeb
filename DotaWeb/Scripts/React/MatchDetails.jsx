﻿// import SearchBar from './SearchBar';

class MatchDetails extends React.Component {
	constructor() {
		super();
		this.state = {
			MatchData: [],
			searchTerm: '',
			search: false,
			loading: false
		}
		this.handleSearchTermSubmit = this.handleSearchTermSubmit.bind(this);
		this.handleSearchTermChange = this.handleSearchTermChange.bind(this);
		this.performSearch = this.performSearch.bind(this);
	}

	handleSearchTermChange(searchTerm) {
		this.setState({ searchTerm, search: false });
	}

	handleSearchTermSubmit() {
		this.setState({ search: true });
	}

	performSearch(searchTerm) {
		// e.preventDefault();
		var isValid = false;
		var errorList = [];
		errorList.push('Enter a valid Match ID or Player ID.');

		var matchID = searchTerm;// this.state.searchTerm;
		var playerID = $('#playerID').val();

		if (!matchID && !playerID)
			alert("Please enter and ID in at least one of the textboxes.");
		else if (!$.isNumeric(matchID) && !$.isNumeric(playerID))
			alert("Only numeric values are allowed.")
		else
			isValid = true;

		if (isValid) {
			this.setState({ loading: true }, () => {
				axios.get("http://localhost:59206/api/products/" + searchTerm).then(response => {
					this.setState({
						loading: false,
						MatchData: response.data
					});
				});
			});
		}
	}

	ShowSpinner() {
		return (
			<div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
				<img src="../Content/Images/Invoke.gif" style={{ width: '100px', height: '100px' }} />
			</div>
		);
	}

	BuildTable(rows) {
		let matchData = this.state.MatchData;
		let showTable = matchData.length == 0 ? { display: 'none' } : {}

		return (
			<section>
				<div style={showTable}>
					{this.BuildMetaTable(matchData)}
					<h1>Players Info</h1>
					{this.BuildTeamTable(rows, 'radiant')}
					{this.BuildTeamTable(rows, 'dire')}
				</div>
			</section>);
	}

	BuildMetaTable(matchData) {
		return (
			<table style={{ width: "100%", textAlign: "center" }}>
				<tbody>
					<tr>
						<td>
							<label htmlFor="Lobbytype">Lobby Type: &nbsp;</label>
							{matchData.Lobbytype}
						</td>
						<td>
							<label htmlFor="Game_Mode">Game Mode: &nbsp;</label>
							{matchData.Game_ModeStr}
						</td>
						<td>
							<label htmlFor="StartTime">Game Start Time: &nbsp;</label>
							{matchData.Start_TimeStr}
						</td>
					</tr>
					<tr>
						<td>
							<label htmlFor="PlayedTimeAgo">Played: &nbsp;</label>
							{matchData.PlayedTimeAgo}
						</td>
						<td>
							<label htmlFor="Duration">Game Duration: &nbsp;</label>
							{matchData.DurationStr}
						</td>
						<td>
							<label htmlFor="First_Blood_TimeStr">First Blood Time: &nbsp;</label>
							{matchData.First_Blood_TimeStr}
						</td>
					</tr>
				</tbody>
			</table>
		);
	}

	BuildTeamTable(rows, team) {
		team == 'radiant' ? rows = rows.slice(0, 5) : rows = rows.slice(5, 10);

		return (
			<table className="table">
				<thead>
					<tr>
						<th>Hero</th>
						<th>Player</th>
						<th>K</th>
						<th>D</th>
						<th>A</th>
						<th>NET</th>
						<th>LH/DN</th>
						<th>GPM/XPM</th>
						<th>DMG</th>
						<th>HEAL</th>
						<th>BLD</th>
						<th>ITEMS</th>
						<th>BACKPACK</th>
					</tr>
				</thead>
				<tbody>
					{rows}
				</tbody>
			</table>
		);
	}

	BuilldRows(matchData, player, index) {
		let teamClass = index > 4 ? "dire" : "radiant";
		let NetWorth = Math.round(player.Gold * matchData.Duration / 1000) + "K";
		let LhDn = player.Last_Hits + " / " + player.Denies;
		let GpXp = player.Gold_Per_Min + " / " + player.Xp_Per_Min;
		let Damage = Math.round(player.Hero_Damage / 1000) + "K";
		return (
			<tr key={index} className={teamClass}>
				<td>
					<img className="img-semi-circle" src={player.HeroImage} alt="HeroImage" />
				</td>
				<td>{player.PlayerName}</td>
				<td>{player.Kills}</td>
				<td>{player.Deaths}</td>
				<td>{player.Assists}</td>
				<td>{NetWorth}</td>
				<td>{LhDn}</td>
				<td>{GpXp}</td>
				<td>{Damage}</td>
				<td>{player.Hero_Healing}</td>
				<td>{player.Tower_Damage}</td>
				<td>
					<img className="img-semi-circle" src={player.Item0Image} alt="Item0 Image" />
					<img className="img-semi-circle" src={player.Item1Image} alt="Item1 Image" />
					<img className="img-semi-circle" src={player.Item2Image} alt="Item2 Image" />
					<img className="img-semi-circle" src={player.Item3Image} alt="Item3 Image" />
					<img className="img-semi-circle" src={player.Item4Image} alt="Item4 Image" />
					<img className="img-semi-circle" src={player.Item5Image} alt="Item5 Image" />
				</td>
				<td>
					<img className="img-semi-circle" src={player.Backpack0Image} alt="Backpack0 Image" />
					<img className="img-semi-circle" src={player.Backpack1Image} alt="Backpack1 Image" />
					<img className="img-semi-circle" src={player.Backpack2Image} alt="Backpack2 Image" />
				</td>
			</tr>);
	}

	render() {
		let rows = [];
		let matchData = this.state.MatchData;
		let playersData = matchData.Players;
		if (playersData !== undefined) {
			playersData.map((player, i) => {
				rows.push(this.BuilldRows(matchData, player, i));
			});
		}

		return (
			<div>
				<SearchBar
					searchTerm={this.state.searchTerm}
					onSearchTermChange={this.handleSearchTermChange}
					onSearchTermSubmit={this.handleSearchTermSubmit}
					performSearch={this.performSearch} />
				{this.state.loading ? this.ShowSpinner() : this.BuildTable(rows)}
			</div>
		);
	}
}

ReactDOM.render(
	<MatchDetails />,
	document.getElementById('myContainer'));