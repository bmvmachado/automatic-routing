import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, headerValue:null };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    handleChange = (e) => {
        this.setState({ headerValue: e.target.value });
    }

    handleUpdate = async () => {
        this.populateWeatherData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        return (
            <>
                <div>
                    <input
                        type="text"
                        value={this.state.headerValue}
                        onChange={this.handleChange}
                        placeholder="Write a headerValue..." />

                    <button
                        onClick={this.handleUpdate}>FetchData</button>
                </div>
                <div>
                    <h1 id="tabelLabel" >Weather forecast</h1>
                    <p>This component demonstrates fetching data from the server.</p>
                    {contents}
                </div>
            </>
        );
    }

    async populateWeatherData() {

        var headers = new Headers();

        if (this.state.headerValue)
        {
            headers.append("Identifier", this.state.headerValue);
        }

        var initReq = {
            method: 'GET',
            headers: headers
        };

        const response = await fetch('weatherforecast/5', initReq);
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
