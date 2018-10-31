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
		//for (var i = 0; i < this.state.ProductData.length; i++) {
		//	let cell = [];
		//	for (var idx = 0; idx < 4; idx++) {
		//		cell.push(<td key={i}>this.state.ProductData.Players[i].Name</td>)
		//	}
		//	rows.push(cell);
		//}
		//console.log(rows);

		return (
			<section>
				<h1>Players List</h1>
				<div>
					<div>
						<table id="radiantTable">
							<thead>
								<tr>
									<th>
										Hero
									</th>
									<th>
										Player
									</th>
									<th>
										K
									</th>
									<th>
										D
									</th>
								</tr>
							</thead>
							<tbody>
								{
									//this.state.ProductData.Players.map((p, index) => {
									//	return <tr key={index}><td>{p.Name}</td><td>{p.PlayerName}</td><td>{p.Kill}</td><td>{p.Deaths}</td></tr>;
									//})
								}
							</tbody>
						</table>
					</div>
				</div>
			</section>
		)
	}
}

ReactDOM.render(
	<ProductsList />,
	document.getElementById('myContainer'));