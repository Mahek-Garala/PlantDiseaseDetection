/*import logo from './logo.svg';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;*/

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Auth from "./components/Auth";
import HomePage from "./components/HomePage";
import Profile from "./components/Profile";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/auth" element={<Auth />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/profile" element={<Profile /> } />
                <Route path="*" element={<Auth />} /> {/* Default to auth page */}
            </Routes>
        </Router>
    );
}

export default App;
