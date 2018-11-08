class SearchBar extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			MatchData: [],
			searchTerm: '',
			search: false
		}
		this.handleSearchTermChange = this.handleSearchTermChange.bind(this);
		this.handleSearchTermSubmit = this.handleSearchTermSubmit.bind(this);
		this.populateGrid = this.populateGrid.bind(this);
	}

	componentDidMount() {
		// this.header.click();
		setTimeout(function () { //Start the timer
			this.header.click(); //After 1 second, set render to true
		}.bind(this), 200)
	}

	populateGrid(searchTerm) {
		this.props.populateGrid(searchTerm);
	}

	handleSearchTermChange(event) {
		this.setState({ searchTerm: event.target.value });
		this.props.onSearchTermChange(event.target.value);
	}

	handleSearchTermSubmit(event) {
		event.preventDefault();
		this.header.click();
		// Is this the best way to get the textbox value?
		// this.props.onSearchTermSubmit(event.target[0].value);

		// Using refs:
		// this.props.onSearchTermSubmit(this.textInput.value);
		this.populateGrid(this.state.searchTerm);
		this.props.onSearchTermSubmit();
	}

	render() {
		return (
			<div className="SearchBar">
				<form onSubmit={this.handleSearchTermSubmit}>
					<div className="jumbotron">
						<h1 ref={input => this.header = input} role="button" data-toggle="collapse" data-target="#collapseHeader" aria-expanded="true" aria-controls="collapseHeader">
							Search match/player
						</h1>
						<div className="collapse" id="collapseHeader">
							<div className="form-horizontal">
								<div className="form-group">
									<label className="col-md-2 control-label" htmlFor="Match_ID">Match ID</label>
									<div className="col-md-10">
										<input className="form-control" id="matchID" name="Match ID" type="text" onChange={this.handleSearchTermChange} />
									</div>
								</div>
								<div className="form-group">
									<label className="col-md-2 control-label" htmlFor="Player_ID">Player ID</label>
									<div className="col-md-10">
										<input className="form-control" id="playerID" name="Player ID" type="text" />
									</div>
								</div>
								<div className="form-group">
									<div className="col-md-offset-2 col-md-10">
										<input type="submit" id="searchButton" value="Search &raquo;" onClick={this.handleSearchTermSubmit} className="btn btn-primary btn-lg" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</form>
			</div>
		);
	}
}

