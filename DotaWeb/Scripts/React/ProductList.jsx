class ProductsList extends React.Component {
	constructor() {
		super();
		this.state = {
			MatchData: []
		}
	}

	componentDidMount() {
		axios.get("http://localhost:59206/api/products/4169885095").then(response => {
			//console.log(response.data.Players.map((p, index) => {
			//	return <tr key={index}><td>{p.Name}</td><td>{p.PlayerName}</td><td>{p.Kill}</td><td>{p.Deaths}</td></tr>;
			//}));
			this.setState({
				MatchData: response.data
			});
		});
	}

	BuildTable(rows) {
		return (
			<section>
				<h1>Players Info</h1>
				<div>
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
							{rows.slice(0,5)}
						</tbody>
					</table>
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
							{rows.slice(5, 10)}
						</tbody>
					</table>
				</div>
			</section>);
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

		return (this.BuildTable(rows))
	}
}

ReactDOM.render(
	<ProductsList />,
	document.getElementById('myContainer'));