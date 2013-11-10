class Server
  include LogHelper

  attr_accessor :clients, :port, :opt
  def initialize(opt = {})
    self.opt = Hashie::Mash.new(opt)
    self.opt.bindip ||= "0.0.0.0"
    self.opt.port ||= 9000
    self.clients = []
  end

  def register(client)
    candidate_id = SecureRandom.hex(6)
    while clients.map(&:id).include?(candidate_id)
      candidate_id = SecureRandom.hex(6)
    end
    client.id = candidate_id
    clients << client
    log :server, "New client #{client.id}"
  end

  def unregister(client)
    clients.delete(client)
    log :server, "Client disconnected #{client.id}"
  end

  def broadcast(data)
    clients.shuffle.each do |client|
      data += "\n" unless data.end_with?("\n")
      client.send_data(data)
    end
  end


  def run
    begin
      EventMachine::run do
        log :info, "Starting server on #{opt.bindip}:#{opt.port}"
        EventMachine::start_server(opt.bindip, opt.port, Client, self)
        log :info, "Started server on #{opt.bindip}:#{opt.port}"
      end
    rescue Interrupt => e
      log :info, "Server interrupted."
    end
  end
end
