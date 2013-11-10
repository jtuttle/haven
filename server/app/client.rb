class Client < EventMachine::Connection
  include LogHelper

  attr_accessor :server, :id
  def initialize(server)
    self.server = server
  end

  def post_init
    server.register self
  end

  def receive_data(rawdata)
    rawdata.split("\n").each do |line|
      log :debug, "[#{id}] received: #{line}"
      data = line.split(' ')
      cmd = data.shift
      if cmd && respond_to?("on_#{cmd}")
        result = send("on_#{cmd}", *data)
        reply result if result
      else
        reply "error what?"
      end
    end
  end

  def unbind
    server.unregister self
    broadcast "drop #{id}"
  end

  def broadcast(*args)
    log "client-#{id}", "broadcast: #{args.join(' ')}"
    server.broadcast(*args)
  end

  def reply(*args)
    log "client-#{id}", "sent: #{args.join(' ')}"
    msg = args.join(' ')
    msg += "\n" unless msg.end_with?("\n")
    send_data(msg)
  end
end
