class ProductsList extends React.Component {
	constructor() {
		super();
		this.state = {
			ProductData: []
		}
	}

	componentDidMount() {
		axios.get("http://localhost:59206/api/products/4169885095").then(response => {
			//console.log(response.data.Players.map((p, index) => {
			//	return <tr key={index}><td>{p.Name}</td><td>{p.PlayerName}</td><td>{p.Kill}</td><td>{p.Deaths}</td></tr>;
			//}));
			this.setState({
				ProductData: response.data
			});
		});
	}

	render() {
		let rows = [];
		let playersData = this.state.ProductData.Players;
		console.log(playersData);
		if (playersData !== undefined) {
			console.log(Object.keys(playersData).length);
			playersData.map((player, i) => {
				let NetWorth = (player.Gold * this.state.ProductData.Duration / 1000) + "K";
				let LhDn = player.Last_Hits + " / " + player.Denies;
				let GpXp = player.Gold_Per_Min + " / " + player.Xp_Per_Min;
				let Damage = player.Hero_Damage / 1000 + "K";
				rows.push(
				<tr key={i}>
					<td>{player.Name}</td>
					<td>{player.PlayerName}</td>
					<td>{player.Kills}</td>
					<td>{player.Deaths}</td>
					<td>{player.Kills}</td>
					<td>{player.Assists}</td>
					<td>{NetWorth}</td>
					<td>{LhDn}</td>
					<td>{GpXp}</td>
					<td>{Damage}</td>
					<td>{player.Hero_Healing}</td>
					<td>{player.Tower_Damage}</td>
					<td>{player.Kills}</td>
				</tr>);
			});
		}

		return (
			<section>
				<h1>Products List</h1>
				<div>
					<table>
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
							{rows
								//this.state.ProductData.Players.map((p, index) => {
								//	return <tr key={index}><td>{p.Name}</td><td> {p.PlayerName}</td><td>{p.Kills}</td><td>{p.Deaths}</td></tr>;
								//})
							}
						</tbody>
					</table>
				</div>


			</section>
		)
	}
}

ReactDOM.render(
	<ProductsList />,
	document.getElementById('myContainer'));