class SearchBar extends React.Component {
	constructor(props) {
		super(props);
		this.handleSearchTermChange = this.handleSearchTermChange.bind(this);
		this.handleSearchTermSubmit = this.handleSearchTermSubmit.bind(this);
	}

	handleSearchTermChange(event) {
		this.props.onSearchTermChange(event.target.value);
	}

	handleSearchTermSubmit(event) {
		event.preventDefault();
		// Is this the best way to get the textbox value?
		// this.props.onSearchTermSubmit(event.target[0].value);

		// Using refs:
		// this.props.onSearchTermSubmit(this.textInput.value);

		this.props.onSearchTermSubmit();
	}

	render() {
		return (
			<div className="SearchBar">
				<h1>SearchBar</h1>
				<form onSubmit={this.handleSearchTermSubmit}>
					{/* <input type="text" /> */}
					{/* <input type="text" ref={(input) => this.textInput = input} /> */}
					<input type="text" onChange={this.handleSearchTermChange} />
					<button>Search</button>
					<button type="button">Random (doesn't do anything)</button>

					<div className="jumbotron">
					<h1 role="button" data-toggle="collapse" data-target="#collapseHeader" aria-expanded="true" aria-controls="collapseHeader">Search match/player</h1>
						<div className="collapse" id="collapseHeader">
							<div className="form-horizontal">
								<div className="form-group">
									<label className="col-md-2 control-label" for="Match_ID">Match IDaa</label>
									<div className="col-md-10">
										<input className="form-control" id="matchID" name="Match ID" type="text" onChange={this.handleSearchTermChange} />
									</div>
								</div>
								<div className="form-group">
									<label className="col-md-2 control-label" for="Player_ID">Player ID</label>
									<div className="col-md-10">
										<input className="form-control" id="playerID" name="Player ID" type="text" />
									</div>
								</div>
								<div className="form-group">
									<div className="col-md-offset-2 col-md-10">
										<input type="submit" id="searchButton" value="Search &raquo;" onClick={this.handleClick} className="btn btn-primary btn-lg" />
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

